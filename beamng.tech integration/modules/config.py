import json

# Load configurations from JSON files
def load_configurations():
    with open('json/configurations.json', 'r') as config_file:
        configurations = json.load(config_file)
    with open('json/internal_configurations.json', 'r') as internal_config_file:
        internal_configurations = json.load(internal_config_file)
    return configurations, internal_configurations


# Validate the input JSON configuration and set defaults
def validate_and_adjust_configuration(input_config):
    # Load configurations
    configurations, internal_configurations = load_configurations()
    
    vehicle_name = input_config.get("vehicle", "etk800")
    parts = input_config.get("parts", {})

    # Check if the vehicle exists
    if vehicle_name not in configurations:
        raise ValueError(f"Vehicle '{vehicle_name}' does not exist in configurations.")

    # Handle default values for ETK800
    if vehicle_name == "etk800":
        # Set default engine if not present
        if "etk_engine" not in parts:
            parts["etk_engine"] = configurations["etk800"]["config"]["etk_engine"][0]

        # Set default transmission if not present
        if "etk_transmission" not in parts:
            parts["etk_transmission"] = configurations["etk800"]["config"]["etk_transmission"][0]

        # Set default front suspension if not present
        if "etk800_strut_F" not in parts and "etk800_strut_F_wide" not in parts:
            parts["etk800_strut_F"] = configurations["etk800"]["config"]["etk800_strut_F"][0]

        # Set default rear suspension if not present
        if "etk800_shock_R" not in parts and "etk800_shock_R_wide" not in parts:
            parts["etk800_shock_R"] = configurations["etk800"]["config"]["etk800_shock_R"][0]

        # Set the fuel tank based on the engine
        fuel_tank_mapping = internal_configurations["etk800"]["parts"]["etk800_fueltank"]
        if parts["etk_engine"] in fuel_tank_mapping:
            parts["etk800_fueltank"] = fuel_tank_mapping[parts["etk_engine"]]

        # Set the front suspension based on strut configuration
        suspension_f_mapping = internal_configurations["etk800"]["parts"]["etk800_suspension_F"]
        for strut in ["etk800_strut_F", "etk800_strut_F_wide"]:
            if strut in parts and parts[strut] in suspension_f_mapping:
                parts["etk800_suspension_F"] = suspension_f_mapping[parts[strut]]
                break

        # Set the rear suspension based on shock configuration
        suspension_r_mapping = internal_configurations["etk800"]["parts"]["etk800_suspension_R"]
        for shock in ["etk800_shock_R", "etk800_shock_R_wide"]:
            if shock in parts and parts[shock] in suspension_r_mapping:
                parts["etk800_suspension_R"] = suspension_r_mapping[parts[shock]]
                break

    # Check if the parts exist
    vehicle_config = configurations[vehicle_name].get("config", {})
    for part_name, part_value in parts.items():
        if part_name not in vehicle_config and part_name not in internal_configurations["etk800"]["parts"]:
            raise ValueError(f"Part '{part_name}' does not exist for vehicle '{vehicle_name}'.")
        if part_name in vehicle_config and part_value not in vehicle_config[part_name]:
            raise ValueError(f"Value '{part_value}' is not valid for part '{part_name}' in vehicle '{vehicle_name}'.")

    # Additional validation for ETK800 (exclusivity of shocks and struts)
    if vehicle_name == "etk800":
        suspension_f_selected = [key for key in ["etk800_strut_F", "etk800_strut_F_wide"] if key in parts]
        suspension_r_selected = [key for key in ["etk800_shock_R", "etk800_shock_R_wide"] if key in parts]

        if len(suspension_f_selected) > 1:
            raise ValueError("ETK800: 'strut_f' and 'strut_f_wide' configurations are exclusive.")
        if len(suspension_r_selected) > 1:
            raise ValueError("ETK800: 'shock_r' and 'shock_r_wide' configurations are exclusive.")

    return (vehicle_name, parts)

import os
import json

# Load configurations from JSON files
def load_configurations(script_dir):    
    json_dir = os.path.join(script_dir, "json")
    with open(os.path.join(json_dir, "configurations.json"), 'r') as config_file:
        configurations = json.load(config_file)
    with open(os.path.join(json_dir, "internal_configurations.json"), 'r') as internal_config_file:
        internal_configurations = json.load(internal_config_file)
    return configurations, internal_configurations


# Validate the input JSON configuration and set defaults
def validate_and_adjust_configuration(input_config, script_dir):
    # Load configurations
    configurations, internal_configurations = load_configurations(script_dir)
    available_parts = {}
    
    vehicle_name = input_config.get("vehicle", "etk800")
    parts = input_config.get("parts", {})

    # Check if the vehicle exists
    if vehicle_name not in configurations:
        raise ValueError(f"Vehicle '{vehicle_name}' does not exist in configurations.")
    else:
        available_parts = configurations[vehicle_name]["config"]
        
    # Switch-like structure for different vehicles
    if vehicle_name == "etk800":
        # Handle defaults and validations for ETK800
        if "etk_engine" not in parts:
            parts["etk_engine"] = available_parts["etk_engine"][0]

        if "etk_transmission" not in parts:
            parts["etk_transmission"] = available_parts["etk_transmission"][0]

        if "etk800_strut_F" not in parts and "etk800_strut_F_wide" not in parts:
            parts["etk800_strut_F"] = available_parts["etk800_strut_F"][0]

        if "etk800_shock_R" not in parts and "etk800_shock_R_wide" not in parts:
            parts["etk800_shock_R"] = available_parts["etk800_shock_R"][0]
        
        if "etk800_brake_F" not in parts:
            parts["etk800_brake_F"] = available_parts["etk800_brake_F"][0]        

        if "etk800_brake_R" not in parts:
            parts["etk800_brake_R"] = available_parts["etk800_brake_R"][0]

        fuel_tank_mapping = internal_configurations[vehicle_name]["parts"]["etk800_fueltank"]
        if parts["etk_engine"] in fuel_tank_mapping:
            parts["etk800_fueltank"] = fuel_tank_mapping[parts["etk_engine"]]

        suspension_f_mapping = internal_configurations[vehicle_name]["parts"]["etk800_suspension_F"]
        for strut in ["etk800_strut_F", "etk800_strut_F_wide"]:
            if strut in parts and parts[strut] in suspension_f_mapping:
                parts["etk800_suspension_F"] = suspension_f_mapping[parts[strut]]
                break

        suspension_r_mapping = internal_configurations[vehicle_name]["parts"]["etk800_suspension_R"]
        for shock in ["etk800_shock_R", "etk800_shock_R_wide"]:
            if shock in parts and parts[shock] in suspension_r_mapping:
                parts["etk800_suspension_R"] = suspension_r_mapping[parts[shock]]
                break
            
    elif vehicle_name == "van":
        # Handle defaults and validations for Van
        if "van_engine" not in parts:
            parts["van_engine"] = available_parts["van_engine"][0]

        if "van_transmission" not in parts:
            parts["van_transmission"] = available_parts["van_transmission"][0]

        if "van_suspension_F" not in parts:
            parts["van_suspension_F"] = available_parts["van_suspension_F"][0]

        if "van_suspension_R" not in parts:
            parts["van_suspension_R"] = available_parts["van_suspension_R"][0]

        if "van_brake_F" not in parts:
            parts["van_brake_F"] = available_parts["van_brake_F"][0]        

        if "van_brake_R" not in parts:
            parts["van_brake_R"] = available_parts["van_brake_R"][0]

        fuel_tank_mapping = internal_configurations[vehicle_name]["parts"]["van_fueltank"]
        if parts["van_engine"] in fuel_tank_mapping:
            parts["van_fueltank"] = fuel_tank_mapping[parts["van_engine"]]
            
    elif vehicle_name == "pickup":
        # Handle defaults and validations for pickup
        if "pickup_engine" not in parts:
            parts["pickup_engine"] = available_parts["pickup_engine"][0]

        if "pickup_transmission" not in parts:
            parts["pickup_transmission"] = available_parts["pickup_transmission"][0]

        if "pickup_suspension_F" not in parts:
            parts["pickup_suspension_F"] = available_parts["pickup_suspension_F"][0]

        if "pickup_suspension_R" not in parts:
            parts["pickup_suspension_R"] = available_parts["pickup_suspension_R"][0]

        if "pickup_brake_F" not in parts:
            parts["pickup_brake_F"] = available_parts["pickup_brake_F"][0]        

        if "pickup_brake_R" not in parts:
            parts["pickup_brake_R"] = available_parts["pickup_brake_R"][0]
            
        fuel_tank_mapping = internal_configurations["pickup"]["parts"]["pickup_fueltank"]
        if parts["pickup_engine"] in fuel_tank_mapping:
            parts["pickup_fueltank"] = fuel_tank_mapping[parts["pickup_engine"]]
    elif vehicle_name == "bolide":
        # Handle defaults and validations for bolide
        if "bolide_engine" not in parts:
            parts["bolide_engine"] = available_parts["bolide_engine"][0]

        if "bolide_coilover_F" not in parts:
            parts["bolide_coilover_F"] = available_parts["bolide_coilover_F"][0]

        if "bolide_coilover_R" not in parts:
            parts["bolide_coilover_R"] = available_parts["bolide_coilover_R"][0]

        if "bolide_brake_F" not in parts:
            parts["bolide_brake_F"] = available_parts["bolide_brake_F"][0]

        if "bolide_brake_R" not in parts:
            parts["bolide_brake_R"] = available_parts["bolide_brake_R"][0]

    # Validate parts against the configuration
    vehicle_config = configurations[vehicle_name].get("config", {})
    for part_name, part_value in parts.items():
        if part_name not in vehicle_config and part_name not in internal_configurations[vehicle_name]["parts"]:
            raise ValueError(f"Part '{part_name}' does not exist for vehicle '{vehicle_name}'.")
        if part_name in vehicle_config and part_value not in vehicle_config[part_name]:
            raise ValueError(f"Value '{part_value}' is not valid for part '{part_name}' in vehicle '{vehicle_name}'.")

    return (vehicle_name, parts)


import os
import json
import modules.simulation as simulation
import modules.config as config
import subprocess

# Automatically install `beamngpy` if not installed
try:
    from beamngpy import BeamNGpy, Vehicle
except ImportError:
    print("beamngpy is not installed. Installing it now...")
    subprocess.check_call([sys.executable, "-m", "pip", "install", "beamngpy"])
    from beamngpy import BeamNGpy, Vehicle

# Main simulation function
def main(beamng_path):
    """
    Main function to initialize and run the BeamNG simulation.

    Args:
        vehicle_config_path (str): Path to the JSON file containing vehicle and parts configuration.
        beamng_path (str): Path to the BeamNG installation.
    """
    # Get the directory of the current script
    script_dir = os.path.dirname(os.path.abspath(__file__))

    # Construct the full path to the JSON configuration file
    json_path = os.path.join(script_dir, "vehicle_config.json")
    
    # Load the vehicle configuration from the JSON file
    with open(json_path, 'r') as file:
        input_config = json.load(file)

    # Validate the configuration and handle defaults
    vehicle_name, vehicle_parts = config.validate_and_adjust_configuration(input_config, script_dir)

    # Initialize BeamNGpy instance with the provided BeamNG path
    beamng_runtime = BeamNGpy('localhost', 64256, home=beamng_path)

    try:
        # Initialize the vehicle and sensors
        vehicle = Vehicle('ego_vehicle', model=vehicle_name)

        # Start the scenario
        simulation.start_scenario(beamng_runtime, vehicle, vehicle_name, vehicle_parts)

        # Run the scenario and capture the result
        result = simulation.run_scenario(vehicle, script_dir)

        print(f"Total Time to Complete Track: {result:.2f} seconds")
    finally:
        # Ensure the simulation is properly closed
        simulation.end_simulation(beamng_runtime)


# Entry point for the script
if __name__ == "__main__":
    import sys
    if len(sys.argv) < 2:
        print("Usage: python script.py '<beamng_path>'")
        sys.exit(1)

    beamng_path = sys.argv[1]
    main(beamng_path)

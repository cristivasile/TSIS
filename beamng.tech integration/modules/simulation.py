from beamngpy import Scenario, Road
from beamngpy.sensors import Electrics
import time, modules.track as track
import matplotlib.pyplot as plt
import os

def find_tsis_folder():
    current_dir = os.path.abspath(os.path.dirname(__file__))
    while True:
        if os.path.basename(current_dir) == "TSIS":
            return current_dir
        parent_dir = os.path.dirname(current_dir)
        if parent_dir == current_dir:  # Reached the root of the filesystem
            raise FileNotFoundError("TSIS folder not found in the parent directories.")
        current_dir = parent_dir

def save_plot_with_incremental_name(folder, base_name):
    # Ensure the folder exists
    if not os.path.exists(folder):
        os.makedirs(folder)

    # Find the highest existing increment
    files = [f for f in os.listdir(folder) if f.startswith(base_name) and f.endswith(".png")]
    increments = [int(f[len(base_name)+1:-4]) for f in files if f[len(base_name)+1:-4].isdigit()]
    next_increment = max(increments, default=0) + 1

    # Construct the new filename
    filename = os.path.join(folder, f"{base_name}_{next_increment}.png")
    return filename

def start_scenario(beamng, vehicle, vehicle_name, vehicle_parts):

    # Launch BeamNG.tech
    beamng.open()

    # Create a fresh scenario on the 'gridmap' map
    scenario = Scenario("gridmap_v2", "road_test")

    # Extract the (x, y, z) coordinates of the start line
    first_segment = track.track[0][:3]  
    second_segment = track.track[-1][:3]

    # Set up sensors
    electrics = Electrics()

    # Attach them
    vehicle.attach_sensor("electrics", electrics)
    
    # Add the vehicle halfway on the finish straight
    scenario.add_vehicle(vehicle, pos=((first_segment[0] + second_segment[0]) / 2, (first_segment[1] + second_segment[1]) / 2, first_segment[2]), rot_quat=(0, 0, 0, 1))

    road = Road("track_editor_C_center", rid="circle_road", looped=True)
    road.add_nodes(*track.track)
    scenario.add_road(road)

    scenario.make(beamng)

    # 4. Generate and load the scenario
    beamng.ui.hide_hud()
    beamng.scenario.load(scenario)
    beamng.scenario.start()

    # dump parts to files
    # dump_parts_config(vehicle, vehicle_name)

    configuration = vehicle.get_part_config()
    
    for part, value in vehicle_parts.items():
        configuration['parts'][part] = value

    vehicle.set_part_config(configuration)

    vehicle.ai_set_mode('span')  # Let AI follow the road
    vehicle.ai_set_aggression(1)

def run_scenario(vehicle, timeout=120):
    start_line = track.track[0][:2]  # Extract the (x, y, z) coordinates of the start line

    # Variables to track timing
    start_time = None
    finished = False
    crossed_start_line_once = False

    # Record the starting point of the timeout
    scenario_start_time = time.time()

    # Variables to log data
    timestamps = []
    speeds = []
    rpms = []
    brakes = []
    fuels = []

    while not finished:
        # Check if timeout has been reached
        elapsed_time = time.time() - scenario_start_time
        if elapsed_time > timeout:
            print("Timeout reached. Scenario not completed.")
            return None

        # Update vehicle position
        vehicle.poll_sensors()
        vehicle_pos = vehicle.state['pos']

        # Log data
        vehicle.sensors.poll()
        sensors = vehicle.sensors
        timestamps.append(elapsed_time)
        speeds.append(sensors['electrics']['airspeed'])
        rpms.append(sensors['electrics']['rpm'])
        brakes.append(sensors['electrics']['brake'])
        fuels.append(sensors['electrics']['fuel'])

        # Check if the vehicle crosses the start line
        if (abs(vehicle_pos[0] - start_line[0]) < 5 and
            abs(vehicle_pos[1] - start_line[1]) < 5):

            # Start the timer the first time the vehicle crosses the start line
            if not crossed_start_line_once:
                start_time = time.time()
                crossed_start_line_once = True
                print("Vehicle crossed the start line.. Start timing")
                time.sleep(5)  # The check will trigger again otherwise
            else:
                # Stop the timer when the vehicle crosses the start line again
                print("Finish!")
                finished = True

        time.sleep(0.01)  # Pause to avoid excessive polling

    # Folder path for saving plots (TSIS/plots)
    tsis_folder = find_tsis_folder()
    plot_folder = os.path.join(tsis_folder, "plots")

    # Speed plot
    plt.figure()
    plt.plot(timestamps, speeds, label='Speed (m/s)')
    plt.xlabel('Time (s)')
    plt.ylabel('Speed (m/s)')
    plt.title('Speed vs Time')
    plt.grid(True)
    plt.legend()
    plt.savefig(save_plot_with_incremental_name(plot_folder, "speed"))
    plt.close()

    # RPM plot
    plt.figure()
    plt.plot(timestamps, rpms, label='RPM', color='blue')
    plt.xlabel('Time (s)')
    plt.ylabel('Engine RPM')
    plt.title('RPM vs Time')
    plt.grid(True)
    plt.legend()
    plt.savefig(save_plot_with_incremental_name(plot_folder, "rpm"))
    plt.close()

    # Brake plot
    plt.figure()
    plt.plot(timestamps, brakes, label='Brake Input', color='orange')
    plt.xlabel('Time (s)')
    plt.ylabel('Brake Input')
    plt.title('Brake Input vs Time')
    plt.grid(True)
    plt.legend()
    plt.savefig(save_plot_with_incremental_name(plot_folder, "brake"))
    plt.close()

    # Fuel plot
    plt.figure()
    plt.plot(timestamps, fuels, label='Fuel Level', color='green')
    plt.xlabel('Time (s)')
    plt.ylabel('Fuel Level')
    plt.title('Fuel Level vs Time')
    plt.grid(True)
    plt.legend()
    plt.savefig(save_plot_with_incremental_name(plot_folder, "fuel"))
    plt.close()

    # Combined plot
    plt.figure(figsize=(12, 10))
    plt.subplot(4, 1, 1)
    plt.plot(timestamps, speeds, label='Speed (m/s)')
    plt.xlabel('Time (s)')
    plt.ylabel('Speed (m/s)')
    plt.title('Speed vs Time')
    plt.grid(True)
    plt.legend()

    plt.subplot(4, 1, 2)
    plt.plot(timestamps, rpms, label='RPM', color='blue')
    plt.xlabel('Time (s)')
    plt.ylabel('Engine RPM')
    plt.title('RPM vs Time')
    plt.grid(True)
    plt.legend()

    plt.subplot(4, 1, 3)
    plt.plot(timestamps, brakes, label='Brake Input', color='orange')
    plt.xlabel('Time (s)')
    plt.ylabel('Brake Input')
    plt.title('Brake Input vs Time')
    plt.grid(True)
    plt.legend()

    plt.subplot(4, 1, 4)
    plt.plot(timestamps, fuels, label='Fuel Level', color='green')
    plt.xlabel('Time (s)')
    plt.ylabel('Fuel Level')
    plt.title('Fuel Level vs Time')
    plt.grid(True)
    plt.legend()

    plt.tight_layout()
    plt.savefig(save_plot_with_incremental_name(plot_folder, "combined"))
    plt.close()

    # Calculate the total time
    end_time = time.time()
    total_time = end_time - start_time if start_time else 0
    return total_time

def end_simulation(beamng):
    beamng.close()
    beamng.disconnect()
    
def dump_parts_config(vehicle, vehicle_name):
    with open(f"{vehicle_name}.json", "+w") as file:
        file.write(str(vehicle.get_part_config()).replace("'", '"'))
    
    with open(f"{vehicle_name}_options.json", "+w") as file:
        dict = vehicle.get_part_options()
        keywords = [vehicle_name, "engine", "transmission", "suspension"]
        filtered_data = {key: value for key, value in dict.items() if any(k in key for k in keywords)}
        file.write(str(filtered_data).replace("'", '"'))
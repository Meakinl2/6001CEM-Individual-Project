
import matplotlib.pyplot as plt
import csv
from datetime import datetime


def Cars100TestingFPS():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/Logs/FPS_Logs/Saved/2025-07-4--22-41-50-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 35000)
        ax.set_xticks([x for x in range(0, 35001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def Cars100TestingCounts():
    x = []
    y_raw = []

    filepath_raw = "GreenFlow/Assets/Logs/Vehicle_Logs/Saved/2025-07-4--22-41-50-vehicle_log.txt"

    with open(filepath_raw, "r") as vehicle_file_raw:
        vehicle_contents_raw = csv.reader(vehicle_file_raw, delimiter=",")

        for row in vehicle_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))


    fig, ax = plt.subplots()


    ax.set_ylim(0,110)
    ax.set_yticks([x for x in range(0, 101, 10)])

    ax.set_xlim(0, 35000)
    ax.set_xticks([x for x in range(0, 35001, 5000)])

    ax.set_xlabel("Milliseconds since Start")
    ax.set_ylabel("Number of Vehicles")

    # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

    ax.grid()

    # ax.set_title("Raw FPS")
    ax.plot(x, y_raw)

    plt.show()

def Cars500TestingFPS():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/Logs/FPS_Logs/Saved/2025-07-4--23-26-54-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 35000)
        ax.set_xticks([x for x in range(0, 35001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def Cars500TestingCounts():
    x = []
    y_raw = []

    filepath_raw = "GreenFlow/Assets/Logs/Vehicle_Logs/Saved/2025-07-4--23-26-54-vehicle_log.txt"

    with open(filepath_raw, "r") as vehicle_file_raw:
        vehicle_contents_raw = csv.reader(vehicle_file_raw, delimiter=",")

        for row in vehicle_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))


    fig, ax = plt.subplots()


    ax.set_ylim(0,510)
    ax.set_yticks([x for x in range(0, 501, 50)])

    ax.set_xlim(0, 30000)
    ax.set_xticks([x for x in range(0, 30001, 5000)])

    ax.set_xlabel("Milliseconds since Start")
    ax.set_ylabel("Number of Vehicles")

    # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

    ax.grid()

    # ax.set_title("Raw FPS")
    ax.plot(x, y_raw)

    plt.show()


def Cars1000TestingFPS():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/Logs/FPS_Logs/Saved/2025-07-4--23-42-19-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 30000)
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def Cars1000TestingCounts():
    x = []
    y_raw = []

    filepath_raw = "GreenFlow/Assets/Logs/Vehicle_Logs/Saved/2025-07-4--23-42-19-vehicle_log.txt"

    with open(filepath_raw, "r") as vehicle_file_raw:
        vehicle_contents_raw = csv.reader(vehicle_file_raw, delimiter=",")

        for row in vehicle_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))


    fig, ax = plt.subplots()


    ax.set_ylim(0,1010)
    ax.set_yticks([x for x in range(0, 1001, 100)])

    ax.set_xlim(0, 35000)
    ax.set_xticks([x for x in range(0, 35001, 5000)])

    ax.set_xlabel("Milliseconds since Start")
    ax.set_ylabel("Number of Vehicles")

    # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

    ax.grid()

    # ax.set_title("Raw FPS")
    ax.plot(x, y_raw)

    plt.show()



def Cars2500TestingFPS():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/Logs/FPS_Logs/Saved/2025-07-4--23-50-07-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 50000)
        ax.set_xticks([x for x in range(0, 50001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def Cars2500TestingCounts():
    x = []
    y_raw = []

    filepath_raw = "GreenFlow/Assets/Logs/Vehicle_Logs/Saved/2025-07-4--23-50-07-vehicle_log.txt"

    with open(filepath_raw, "r") as vehicle_file_raw:
        vehicle_contents_raw = csv.reader(vehicle_file_raw, delimiter=",")

        for row in vehicle_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))


    fig, ax = plt.subplots()


    ax.set_ylim(0,2510)
    ax.set_yticks([x for x in range(0, 2501, 250)])

    ax.set_xlim(0, 50000)
    ax.set_xticks([x for x in range(0, 50001, 5000)])

    ax.set_xlabel("Milliseconds since Start")
    ax.set_ylabel("Number of Vehicles")

    # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

    ax.grid()

    # ax.set_title("Raw FPS")
    ax.plot(x, y_raw)

    plt.show()



def Cars5000TestingFPS():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/Logs/FPS_Logs/Saved/2025-07-4--23-54-20-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 130000)
        ax.set_xticks([x for x in range(0, 130001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def Cars5000TestingCounts():
    x = []
    y_raw = []

    filepath_raw = "GreenFlow/Assets/Logs/Vehicle_Logs/Saved/2025-07-4--23-54-20-vehicle_log.txt"

    with open(filepath_raw, "r") as vehicle_file_raw:
        vehicle_contents_raw = csv.reader(vehicle_file_raw, delimiter=",")

        for row in vehicle_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))


    fig, ax = plt.subplots()


    ax.set_ylim(0,5100)
    ax.set_yticks([x for x in range(0, 5001, 500)])

    ax.set_xlim(0, 130000)
    ax.set_xticks([x for x in range(0, 130001, 10000)])

    ax.set_xlabel("Milliseconds since Start")
    ax.set_ylabel("Number of Vehicles")

    # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

    ax.grid()

    # ax.set_title("Raw FPS")
    ax.plot(x, y_raw)

    plt.show()


def Cars10000TestingFPS():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/Logs/FPS_Logs/Saved/2025-08-4--00-08-54-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 430000)
        ax.set_xticks([x for x in range(0, 430001, 50000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def Cars10000TestingCounts():
    x = []
    y_raw = []

    filepath_raw = "GreenFlow/Assets/Logs/Vehicle_Logs/Saved/2025-08-4--00-08-54-vehicle_log.txt"

    with open(filepath_raw, "r") as vehicle_file_raw:
        vehicle_contents_raw = csv.reader(vehicle_file_raw, delimiter=",")

        for row in vehicle_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))


    fig, ax = plt.subplots()


    ax.set_ylim(0,10100)
    ax.set_yticks([x for x in range(0, 10001, 1000)])

    ax.set_xlim(0, 430000)
    ax.set_xticks([x for x in range(0, 430001, 50000)])

    ax.set_xlabel("Milliseconds since Start")
    ax.set_ylabel("Number of Vehicles")

    # ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

    ax.grid()

    # ax.set_title("Raw FPS")
    ax.plot(x, y_raw)

    plt.show()


Cars10000TestingFPS()
Cars10000TestingCounts()

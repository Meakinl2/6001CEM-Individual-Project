import matplotlib.pyplot as plt
import csv
from datetime import datetime

def BezierCurvesInitial():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/2025-31-3--14-51-41-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    print(x)
    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 35000)
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([3500, 8500, 12300, 16625, 21500, 26350], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def BezierCurvesWithoutSubnodes():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/2025-31-3--21-36-52-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    print(x)
    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 35000)
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([4025, 8800, 12900, 19000, 24000, 29700], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()


def BezierCurvesParallelised():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/2025-31-3--22-41-18-fps_log_raw.txt"

    with open(filepath_raw, "r") as fps_file_raw:
        fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

        for row in fps_contents_raw:
            x.append(float(row[0]))
            y_raw.append(float(row[1]))
            y_sm1.append(float(row[2]))

    fig, [ax1,ax2] = plt.subplots(ncols=1, nrows=2)

    print(x)
    for ax in [ax1,ax2]:
        ax.set_ylim(0,1500)
        ax.set_yticks([x for x in range(0, 1501, 100)])

        ax.set_xlim(0, 35000)
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([4025, 8800, 12900, 19000, 24000, 29700], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()


BezierCurvesParallelised()




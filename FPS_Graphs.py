import matplotlib.pyplot as plt
import csv
from datetime import datetime

def BezierCurvesInitial():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-31-3--14-51-41-fps_log_raw.txt"

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
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-31-3--21-36-52-fps_log_raw.txt"

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



def BezierCurvesSequential8Nodes():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-01-4--10-55-53-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([5750, 15975, 21000, 31000], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()


def BezierCurvesParallel():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-01-4--15-55-36-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([3075, 7050, 11200, 16500, 20100, 26000], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def BezierCurvesParallel8Nodes():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-01-4--16-13-03-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([2325, 11200, 16500, 24000], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def BezierCurvesParallelLong():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-02-4--12-39-01-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([4250, 9000, 13500, 19250, 24750,30500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def BezierCurvesParallelLong8Nodes():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-02-4--12-50-08-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([4500, 13750, 18500, 24250], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()


def BezierCurvesLanes1():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/Saved/2025-03-4--11-05-01-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([4950, 12500, 17000, 24700], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def BezierCurvesLanes10():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/2025-03-4--11-12-05-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([4750, 13000, 20000, 28000], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()



def BezierCurvesLanes10():
    x = []
    y_raw = []
    y_sm1 = []
    filepath_raw = "GreenFlow/Assets/FPS_Logs/2025-03-4--11-25-02-fps_log_raw.txt"

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
        ax.set_xticks([x for x in range(0, 30001, 5000)])

        ax.set_xlabel("Milliseconds since Start")
        ax.set_ylabel("FPS")

        ax.vlines([5500, 11000, 18000, 24500], 0, 1500, color="r")

        ax.grid()

    ax1.set_title("Raw FPS")
    ax2.set_title("Smoothed FPS")

    ax1.scatter(x, y_raw)
    ax2.scatter(x, y_sm1)
    plt.show()


BezierCurvesLanes10()




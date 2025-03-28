import matplotlib.pyplot as plt
import csv
from datetime import datetime

x = []
y_raw = []
y_sm1 = []
y_sm2 = []
y_sm3 = []
y_sm4 = []
y_sm5 = []

filepath_raw = "GreenFlow/Assets/FPS_Logs/2025-28-3--00-22-37-fps_log_raw.txt"


with open(filepath_raw, "r") as fps_file_raw:
    fps_contents_raw = csv.reader(fps_file_raw, delimiter=",")

    for row in fps_contents_raw:
        x.append(datetime.strptime(row[0], "%Y-%d-%m--%H-%M-%S"))
        y_raw.append(float(row[1]))
        y_sm1.append(float(row[2]))
        y_sm2.append(float(row[3]))
        y_sm3.append(float(row[4]))
        y_sm4.append(float(row[5]))
        y_sm5.append(float(row[6]))


fig, [[ax1,ax2,ax3],[ax4,ax5,ax6]] = plt.subplots(ncols=3, nrows=2)


for ax in [ax1,ax2,ax3,ax4,ax5,ax6]:
    ax.ylim = 0,1500
    ax.set_xticks([x for x in range(0, 1501, 100)])

ax1.plot(x, y_raw)
ax2.plot(x, y_sm1)
ax3.plot(x, y_sm2)
ax4.plot(x, y_sm3)
ax5.plot(x, y_sm4)
ax6.plot(x, y_sm5)
plt.show()



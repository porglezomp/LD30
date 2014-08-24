#!/usr/bin/env python
from subprocess import call
import os
import time

#variables
path = "timelapse/"
folder = ""
extension = ".png"
command = "screencapture"

#Get a new project name
while folder == "":
	#what do you want the project to be called
	folder = input("Timelapse name: ")
	#is there already a folder with that name?
	if os.path.exists(path + folder):
		#if so, don't allow creation
		print("That already exists")
		folder = ""

#create the needed folders
os.makedirs(path + folder)

#get the framerate
SPF = float(input("Seconds per frame: "))
#print some info
print("Type Ctrl-C to finish")
#wait a moment so that the terminal window isn't in front
try:
	time.sleep(1.5)
	print("GO!")
except (KeyboardInterrupt, SystemExit):
	print("Cancelling")

frame = 0
while True:
	try:
		#take a screenshot
		call([command, "-x", path + folder + "/frame" + '{:04}'.format(frame) + extension])
		#increment the frame number for the filename to use
		frame += 1
		#wait for the framerate
		time.sleep(SPF)
	except:
		print("Timelapse complete after", frame, "frames.")
		break

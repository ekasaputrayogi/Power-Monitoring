# This file is executed on every boot (including wake-boot from deepsleep)
#import esp
#esp.osdebug(None)
#import webrepl
#webrepl.start()import machine
import WifiConnect


while True:
    WifiConnect.connecttd()
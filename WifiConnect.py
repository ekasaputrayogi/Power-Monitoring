import machine
from machine import Pin,ADC
import time
import network
import os
from umqtt.simple import MQTTClient


leds = machine.Pin(12, machine.Pin.OUT)
led = machine.Pin(2, machine.Pin.OUT)
leds.on()
adc = ADC(Pin(34))
adc.atten(ADC.ATTN_11DB)
adc.width(ADC.WIDTH_12BIT)
ldrvalue=0


SERVER = "mqtt.zstel.com"
user_password = "gtfl$136073"
user_name = 'FAC_GT_#13'
CLIENT_ID = 'ITFW_TEST1'
PORT = 1883
client = MQTTClient(CLIENT_ID, SERVER, 0, user_name, user_password,60)

uart = machine.UART(2, 115200)
strMsg = ''
mode = ""
sta_if = network.WLAN(network.STA_IF)
config_array = []
config = ""


def call_back_function(topic, msg):
    message = msg.decode().strip("'\n")
    print(message)
    try:
        if message == 'on':
            print("ON")
            led.on()
        elif message == 'off':
            led.off()
    except:
        pass
message = ""
client.set_callback(call_back_function) 
def connecttd():
    global message, strMsg, config_array, config
    ssid = "ac55"
    password =  ""
    
       
    station = network.WLAN(network.STA_IF)
    
    if station.isconnected() == True:
        #print("Already connected")
        led.on()
        client.connect()
        client.subscribe("TESA")
        while True:
                ldrvalue=adc.read()
                if station.isconnected() == False:
                    #client.disconnect()
                    return
                client.publish("IOTBOX", str(ldrvalue))
                client.check_msg()
                #print(ldrvalue)
                
                time.sleep(0.5)
                
                #print(message)
        return
    f = open("demofile2.txt", "r")
    config = f.read()
    
    config_split = config.splitlines()
    f.close()
    print(config_split)
  
    
    station.active(False)
    station.active(True)
    
    station.connect(config_split[0],config_split[1])
    while station.isconnected() == False:
        
        if uart.any() > 0:
            # Read all the character to strMsg variable
            #os.remove("demofile.txt")
            led.off()
            f = open("demofile2.txt", "w")
            strMsg = uart.read()
            conf = f.write(strMsg)
            
            f.close()
            f = open("demofile2.txt", "r")
            config = f.read()
            config_split = config.splitlines()
            try:
                station.connect(config_split[0],config_split[1])
            except:
                pass
            #print(config)
            f.close()
        else:
            led.off()
            pass
            
 
    print("Connection successful")
    print(station.ifconfig())

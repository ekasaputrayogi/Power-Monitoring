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



uart = machine.UART(2, 115200)
config_split= []

def Start():
    
        
        global config_split
        station = network.WLAN(network.STA_IF)
        
        if station.isconnected() == True:
            print("Connection successful")
            print(station.ifconfig())

            led.on()
            print(config_split[6])            
            pin0_split = config_split[6].split()
            pin0 = ADC(Pin(34))
            pin0.atten(ADC.ATTN_11DB)
            pin0.width(ADC.WIDTH_12BIT)

            server = config_split[2]
            user_name = config_split[3]
            user_password = config_split[4]
            port = config_split[5]
            try:
                client = MQTTClient("umqtt_client",server)
                client.connect()
            except:
                return
            while True:
                pin0_ = pin0.read()
                pin0_data = (int(pin0_split[5]) - int(pin0_split[4])/4095)*pin0_ + int(pin0_split[4])
                print(pin0_data)
                if station.isconnected() == False:
                    return
                #if client.connect(False):
                #    return
                client.publish("IOTBOX", str(pin0_data))
                time.sleep(0.5)
                #print("itu")
            return
        
        f = open("config.txt", "r")
        config = f.read()
        config_split = config.splitlines()
        f.close()
        print(config_split)
        station.active(False)
        
        station.active(True)        
        station.connect(config_split[0],config_split[1])
        
        while station.isconnected() == False:
            
            if uart.any() > 0:
                led.off()
                f = open("config.txt", "w")
                strMsg = uart.read()
                conf = f.write(strMsg)
                f.close()
                f = open("config.txt", "r")
                config = f.read()
                config_split = config.splitlines()
                try:
                    station.connect(config_split[0],config_split[1])
                except:
                    pass
                f.close()
            else:
                led.off()
                pass
                

        


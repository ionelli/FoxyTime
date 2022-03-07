using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class PetArduinoHandler : MonoBehaviour
{
    private SerialController _serial;

    public PhysicalPetState State { get; private set; }

    void Awake()
    {
        _serial = GetComponent<SerialController>();
        InitSerialPort();
        _serial.enabled = true;
    }

    private void InitSerialPort()
    {
        string port="";
        
        foreach (string openPort in SerialPort.GetPortNames())
        {
            print(openPort);
            if (openPort != "COM1") { port = openPort; break; }
        }

        _serial.portName = port;
        _serial.messageListener = gameObject;
    }

    private void OnMessageArrived(string msg)
    {
        State = JsonUtility.FromJson<PhysicalPetState>(msg);
        //print(State.ToJson());
    }

    private void OnConnectionEvent(bool success)
    {
        print("Connection: " + (success ? "success" : "failure"));
    }
}

public struct PhysicalPetState
{
    public float distance;
    public float flexAngleLeft;
    public float flexAngleRight;
    public bool motionDetected;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

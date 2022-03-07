using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class PetArduinoHandler : MonoBehaviour
{
    private SerialPort _serial;
    private PhysicalPetState _state;
    private bool _waitingForState;

    public PhysicalPetState State => _state;
    
    void Awake()
    {
        InitSerialPort();
    }

    void Start()
    {
        _serial.DataReceived += SaveCurrentState;
    }

    private void SaveCurrentState(object sender, SerialDataReceivedEventArgs e)
    {
        _state = JsonUtility.FromJson<PhysicalPetState>(e.ToString());
    }

    void FixedUpdate()
    {
        if(!_serial.IsOpen)
            _serial.Open();

        if (!_waitingForState)
            _serial.Write("0"); //we dont ask for multiple things, just asking for any
    }

    private void InitSerialPort()
    {
        string port="";
        
        foreach (string openPort in SerialPort.GetPortNames())
        {
            print(openPort);
            if (openPort != "COM1") { port = openPort; break; }
        }
        _serial = new SerialPort(port, 9600);
        if (!_serial.IsOpen)
        {
            print("Opening " + port + ", baud 9600");
            _serial.Open();
            _serial.ReadTimeout = 100;
            _serial.Handshake = Handshake.None;
            if (_serial.IsOpen) { print("Open"); }
        }
    }
}

public struct PhysicalPetState
{
    public float distance;
    public float flexAngleLeft;
    public float flexAngleRight;
    public bool motionDetected;
}

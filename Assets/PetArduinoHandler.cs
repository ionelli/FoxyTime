using System.IO.Ports;
using UnityEngine;

public class PetArduinoHandler : MonoBehaviour
{
    [SerializeField] private bool spoofData;
    private SerialController _serial;

    public PhysicalPetState State { get; private set; }

    void Awake()
    {
        _serial = GetComponent<SerialController>();
        InitSerialPort();
        _serial.enabled = true;
        enabled = spoofData;
    }

    private float _spoofTimer;
    private void Update()
    {
        if (_spoofTimer < 1f)
        {
            _spoofTimer += Time.deltaTime;
            return;
        }
        
        State = new PhysicalPetState()
        {
            distance = Random.Range(PhysicalPetState.minDistance, PhysicalPetState.maxDistance)
        };

        _spoofTimer = 0;
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
    public const float minDistance = 2f;
    public const float maxDistance = 3000f;

    public float distance;
    public float flexAngleLeft;
    public float flexAngleRight;
    public bool motionDetected;
    public float normalizedDistance => Mathf.InverseLerp(minDistance, maxDistance, distance);

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

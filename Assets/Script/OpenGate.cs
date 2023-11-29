using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    private AudioSource m_Source;
    [SerializeField]
    private AudioClip openning;
    [SerializeField]
    private AudioClip locked;
    [SerializeField]
    private GameObject gate;
    [SerializeField]
    private bool hasKey;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private string nameKey;
    private Animator GATE1;
    [SerializeField]
    private bool open = false;


    // Start is called before the first frame update
    void Start()
    {
        GATE1 = gate.GetComponent<Animator>();
        this.AddComponent<AudioSource>();
        m_Source = GetComponent<AudioSource>();
    }

    public void GateOpen()
    {
        if (!open && isLocked)
        {
            GATE1.SetTrigger("OpenGate");
            GATE1.SetBool("GateOpen", true);
            if (!m_Source.isPlaying)
            {
                m_Source.PlayOneShot(locked, 0.3f);
            }
            Debug.Log("Gate locked!");
        }
        else if (!open && !isLocked)
        {
            GATE1.SetBool("HasKey", true);
            GATE1.SetTrigger("OpenGate");
            GATE1.SetBool("DoorGate", true);
            if (!m_Source.isPlaying)
            {
                m_Source.PlayOneShot(openning, 0.3f);
            }
            Debug.Log("Abrindo Fechada");
        }
        else if (open)
        {
            GATE1.SetBool("DoorOpen", false);
            GATE1.SetTrigger("OpenDoor");
            Debug.Log("Gate open!");
        }

        open = !open;
    }

    public bool HasKey(string name)
    {
        if (nameKey.Equals(name))
        {
            hasKey = true;
            return true;
        }

        return false;
    }

    public bool GateIsLocked()
    {
        if (isLocked)
        {
            return true;
        }

        return false;
    }

    public void UnlockGate()
    {
        if (isLocked)
        {
            isLocked = false;
        }
    }
}

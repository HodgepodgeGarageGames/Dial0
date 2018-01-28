using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchboard {
    public List<IncomingCalls> allCalls = new List<IncomingCalls>();

    public enum Exchange
    {
        KLONDIKE1 = 0,
        KLONDIKE2,
        KLONDIKE3,
        KLONDIKE4,
        HOVELTON1,
        HOVELTON2,
        HOVELTON3,
        HOVELTON4,
        KLONDIKE5,
        KLONDIKE6,
        KLONDIKE7,
        KLONDIKE8,
        HOVELTON5,
        HOVELTON6,
        HOVELTON7,
        HOVELTON8,
        KLONDIKE9,
        KLONDIKE10,
        KLONDIKE11,
        KLONDIKE12,
        HOVELTON9,
        HOVELTON10,
        HOVELTON11,
        HOVELTON12,
        ROSEHEIGHTS1,
        ROSEHEIGHTS2,
        ROSEHEIGHTS3,
        ROSEHEIGHTS4,
        PEACOCK1,
        PEACOCK2,
        PEACOCK3,
        PEACOCK4,
        ROSEHEIGHTS5,
        ROSEHEIGHTS6,
        ROSEHEIGHTS7,
        ROSEHEIGHTS8,
        PEACOCK5,
        PEACOCK6,
        PEACOCK7,
        PEACOCK8,
        ROSEHEIGHTS9,
        ROSEHEIGHTS10,
        ROSEHEIGHTS11,
        ROSEHEIGHTS12,
        PEACOCK9,
        PEACOCK10,
        PEACOCK11,
        PEACOCK12,
    }

    public void initCalls()
    {
        allCalls = new List<IncomingCalls>()
        {
            new IncomingCalls() {
                Audio = Resources.Load("Audio/Calls/RileyRoseHeights1") as AudioClip,
                Destination = (int)Exchange.ROSEHEIGHTS1
            },
            new IncomingCalls() {
                Audio = Resources.Load("Audio/Calls/AliceMcNeelyHov6") as AudioClip,
                Destination = (int)Exchange.HOVELTON6
            },
            new IncomingCalls() {
                Audio = Resources.Load("Audio/Calls/MotherHovelton3") as AudioClip,
                Destination = (int)Exchange.HOVELTON3
            },
            new IncomingCalls() {
                Audio = Resources.Load("Audio/Calls/OdayKlondike2") as AudioClip,
                Destination = (int)Exchange.KLONDIKE2
            },
        };
    }
}

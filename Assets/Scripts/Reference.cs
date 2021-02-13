using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Reference : MonoBehaviour
{
    private static readonly Reference instance = new Reference();
    public BoardManager boardManager;

    static Reference() {}

    private Reference() {}

    void Awake() {}

    public static Reference GetInstance() {
        return instance;
    }
}

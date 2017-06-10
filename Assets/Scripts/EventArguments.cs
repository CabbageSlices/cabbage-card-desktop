using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// argumetns for response event to draw card event
/// </summary>
public class DrawCardResponseArgs : System.EventArgs {
    public GameObject card; ///<Remark>card that was drawn</Remark>
}
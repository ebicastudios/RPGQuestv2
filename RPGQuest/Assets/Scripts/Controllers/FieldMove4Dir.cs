using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;




public class FieldMove4Dir : MonoBehaviour {

    #region PUBLIC-VARS
    public NAVMODE navMode;

    [Header("Components")]
    public StateManager stateManager;
    public Settings settings;
    public Rigidbody2D rigidbody;

    [Header("Flags and Modifiers")]
    public bool isEnabled;
    public float walkSpeed;

    [Header("KeyMap")]
    public string up;
    public string down;
    public string left;
    public string right;
    public string cycle;

    #endregion
    #region PRIVATE-VARS
    private Vector2 velocity;
    private Vector3 position;
    #endregion
    #region INITIALIZATION-KEYSETS
    void Awake()
    {
        if(stateManager == null) { throw new System.Exception("FATAL: stateManager not attached to FieldMove4Dir script on " + this.gameObject.name + ". Fatal Error."); } //No State Manager attached
        if(settings == null) { throw new System.Exception("FATAL: settings not attached to FieldMove4Dir script on " + this.gameObject.name + ". Fatal Error."); }  //No Settings Attached
        else { setKeys(); }
        if(rigidbody == null)
        {
            rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
            if(rigidbody == null) { throw new System.Exception("FATAL: rigidbody not attached to FieldMove4Dir script on " + this.gameObject.name + ". Fatal Error"); } //No rigidbody2D attached, and couldn't find on on the gameObject
        }
        
    }
    public void setKeys()
    {
        up = settings.keyUp;
        down = settings.keyDown;
        left = settings.keyLeft;
        right = settings.keyRight;
        cycle = settings.cycle;
    }
    #endregion
    #region UPDATE-FUNCTIONS
    void FixedUpdate()
    {
        if(stateManager.gameState == GAMESTATE.Explore) { isEnabled = true; }
        else { isEnabled = false; }
        if (isEnabled)
        {
            if(navMode == NAVMODE.Fixed) { fixedPath(); }
            else if(navMode == NAVMODE.Freeze) { freezePath(); }
            else if(navMode == NAVMODE.Random) { randomPath(); }
            else if(navMode == NAVMODE.Follow) { followPath(); }
            else
            {
                if (Input.GetKey(down)) { downEvent(); }
                if (Input.GetKey(up)) { upEvent(); }
                if (Input.GetKey(left)) { leftEvent(); }
                if (Input.GetKey(right)) { rightEvent(); }

            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(cycle)) { cyclePartyPosition(); }
        //HACK: Ensure that the Player GameObject doesn't cycle Z-layers, only its children. Workaround by constantly setting the z level to 0.
        position = this.GetComponent<Transform>().position;
        position.z = 0;
        this.GetComponent<Transform>().position = position;
    }
    #endregion
    #region NON-PLAYER-NAV-FUNCTIONS(NOTALLIMPLEMENTED)
    void fixedPath() { }
    void randomPath() { }
    void freezePath() { }
    void followPath() { }
    void cyclePartyPosition()
    {
        Transform[] tempTransform;
        tempTransform = this.GetComponentsInChildren<Transform>();
        foreach(Transform trans in tempTransform)
        {
            Vector3 position = new Vector3(0, 0, 0);
            position = trans.localPosition;
            position.z -= 1;
            if(position.z < 0) { position.z = 4; }
            Debug.Log("New Vec Z:" + position.z);
            trans.localPosition = position;
        }
    }
    #endregion
    #region MOVE-EVENTS
    void leftEvent() {
        velocity = rigidbody.velocity;
        velocity.x -= walkSpeed;
        rigidbody.velocity = velocity;
    }
    void rightEvent() {
        velocity = rigidbody.velocity;
        velocity.x += walkSpeed;
        rigidbody.velocity = velocity;
    }
    void downEvent() {
        velocity = rigidbody.velocity;
        velocity.y -= walkSpeed;
        rigidbody.velocity = velocity;
    }
    void upEvent() {
        velocity = rigidbody.velocity;
        velocity.y += walkSpeed;
        rigidbody.velocity = velocity;
    }
    #endregion
}

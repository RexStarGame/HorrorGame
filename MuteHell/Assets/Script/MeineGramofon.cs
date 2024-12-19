using UnityEngine;

public class MeineGramofon : MonoBehaviour
{
    public bool start;
    bool isitleft;
    bool firstbounce;
    bool moveup;
    public float speed = 1;
    Vector3 rs;
    Vector3 ps;
    Transform ts;
    Transform t;
    Vector3 p;
    Vector3 r;
    // Start is called before the first frame update
    void Start()
    {
        ts = transform;
        ps = ts.position;
        rs = ts.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        t = transform;
        p = t.position;
        r = t.eulerAngles;
        if (Input.GetKey(KeyCode.E))
        {
            start = true;
            firstbounce = true;
            moveup = true;
            t = ts;
            r = rs;
            p = ps;
            Debug.Log("you pressed E");
        }

        if (start)
        {
            Debug.Log("animation started");

            if (firstbounce)
            {
                Debug.Log("first bounce initiated");
                if (r.z == 20)
                {
                    firstbounce = false;
                    Debug.Log("first bounce complete");
                }
                else if (moveup)
                {
                    gameObject.transform.position = new Vector3(p.x, p.y + 0.3f, p.z);
                    moveup = false;
                    Debug.Log("moved up");
                }
                else
                {

                    r = Vector3.Lerp(r, new Vector3(r.x, r.y, 20), Time.deltaTime);
                    float z = r.z;
                    Debug.Log("rotation.z changed");
                    Debug.Log(z);
                }
            }
        }
    }
}

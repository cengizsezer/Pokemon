using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayExtentions
{

    public static Vector3 GetRayHitPointToObject(this Ray ray, string _tag, Transform target)
    {
        RaycastHit hit;
        Vector3 v = Vector3.zero;

        ray = new Ray(Camera.main.transform.position, target.position - Camera.main.transform.position);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider.CompareTag(_tag))
            {
                v = hit.point;
            }
        }

        return v;
    }

    public static Vector3 GetRayHitPointOfMouse(this Ray ray, string _tag)
    {
        RaycastHit hit;
        Vector3 v = Vector3.zero;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(_tag))
                {
                    v = hit.point;
                    Debug.Log("HIT FOUND :" + hit.collider.gameObject.name + " ====> " + v);
                }
                else
                    Debug.Log("Tag Not Matched");
            }
            else
            {
                Debug.Log("No Hit Collision");
            }
        }
        else
            Debug.Log("No Hit Collision 2");


        return v;
    }

    public static Vector3 GetRayHitPoint(this Ray ray, string _tag, GameObject go, Camera cam)
    {
        RaycastHit hit;
        Vector3 v = Vector3.zero;

        ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (!string.IsNullOrEmpty(_tag))
            {
                if (hit.collider.CompareTag(_tag))
                {
                    if (go == null)
                        return hit.point;
                    else if (go == hit.collider.gameObject)
                        return hit.point;
                }
            }
            else
            {
                if (go == null)
                    return hit.point;
                else if (go == hit.collider.gameObject)
                    return hit.point;
            }
        }

        return v;
    }

    public static Vector3 GetRayHitPoint(this Ray ray, string _tag) => GetRayHitPoint(ray, _tag, null, Camera.main);

    public static Vector3 GetRayHitPoint(this Ray ray, string _tag, GameObject go) => GetRayHitPoint(ray, _tag, go, Camera.main);

    public static Vector3 GetRayHitPoint(this Ray ray, string _tag, Camera cam) => GetRayHitPoint(ray, _tag, null, cam);

    public static Vector3 GetRayHitPoint(this Ray ray, GameObject go) => GetRayHitPoint(ray, "", go, Camera.main);

    public static Vector3 GetRayHitPoint(this Ray ray, GameObject go, Camera cam) => GetRayHitPoint(ray, "", go, cam);

    public static bool IsMouseOverObject(this Ray ray, string _tag, GameObject go, Camera cam)
    {
        bool onObj = false;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (!string.IsNullOrEmpty(_tag))
            {
                if (hit.collider.CompareTag(_tag))
                {
                    if (go == null)
                        onObj = true;
                    else if (go == hit.collider.gameObject)
                        onObj = true;
                }
            }
            else
            {
                if (go == null)
                    onObj = true;
                else if (go == hit.collider.gameObject)
                    onObj = true;
            }
        }


        return onObj;
    }

    public static bool IsMouseOverObject(this Ray ray, string _tag) => IsMouseOverObject(ray, _tag, null, Camera.main);

    public static bool IsMouseOverObject(this Ray ray, string _tag, GameObject go) => IsMouseOverObject(ray, _tag, go, Camera.main);

    public static bool IsMouseOverObject(this Ray ray, string _tag, Camera cam) => IsMouseOverObject(ray, _tag, null, cam);

    public static bool IsMouseOverObject(this Ray ray, GameObject go) => IsMouseOverObject(ray, "", go, Camera.main);

    public static bool IsMouseOverObject(this Ray ray, GameObject go, Camera cam) => IsMouseOverObject(ray, "", go, cam);

    public static T GetObjectWithMouseRay<T>(this Ray ray, string _tag, LayerMask layer)
    {
        T t = default;
        RaycastHit hit;

        if (Camera.main == null) Debug.Log("ITS NULL");

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            if (hit.collider.CompareTag(_tag))
            {
                t = hit.collider.GetComponent<T>();
            }
        }

        return t;
    }

    public static T GetObjectWithMouseRay<T>(this Ray ray, string _tag) => GetObjectWithMouseRay<T>(ray, _tag, LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

    public static T GetObjectWithRayToObject<T>(this Ray ray, string _tag, Transform target, LayerMask layer)
    {
        T t = default;
        RaycastHit hit;
        ray = new Ray(Camera.main.transform.position, target.position - Camera.main.transform.position);

        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            if (hit.collider.CompareTag(_tag))
            {
                t = hit.collider.GetComponent<T>();
            }
        }

        return t;
    }

    public static T GetObjectWithRayToObject<T>(this Ray ray, string _tag, Transform target) => GetObjectWithRayToObject<T>(ray, _tag, target, LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

    public static List<T> GetObjectsWithRayToObject<T>(this Ray ray, string _tag, Transform target, LayerMask layer)
    {
        List<T> ls = new List<T>();

        ray = new Ray(Camera.main.transform.position, target.position - Camera.main.transform.position);
        RaycastHit[] allHits = Physics.RaycastAll(ray, 100f, layer);
        if (allHits.Length > 0)
        {
            foreach (RaycastHit h in allHits)
            {
                if (h.collider.CompareTag(_tag))
                {
                    T t = h.collider.GetComponent<T>();

                    if (t != null)
                    {
                        ls.Add(t);
                    }
                }
            }
        }

        return ls;
    }

    public static List<T> GetObjectsWithRayToObject<T>(this Ray ray, string _tag, Transform target) => GetObjectsWithRayToObject<T>(ray, _tag, target, LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

    public static List<T> GetObjectsWithMouseRay<T>(this Ray ray, string _tag, LayerMask layer)
    {
        List<T> ls = new List<T>();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] allHits = Physics.RaycastAll(ray, 100f, layer);
        if (allHits.Length > 0)
        {
            foreach (RaycastHit h in allHits)
            {
                if (h.collider.CompareTag(_tag))
                {
                    T t = h.collider.GetComponent<T>();

                    if (t != null)
                    {
                        ls.Add(t);
                    }
                }
            }
        }

        return ls;
    }

    public static List<T> GetObjectsWithMouseRay<T>(this Ray ray, string _tag) => GetObjectsWithMouseRay<T>(ray, _tag, LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

    public static Vector3 GetWorldPositionOnPlaneX(this Ray ray, float x)
    {
        Camera activeCam = Camera.main;
        Vector3 camPos = activeCam.transform.position;
        ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Plane yz = new Plane(Vector3.forward, new Vector3(x, camPos.y, camPos.z));
        float distance;
        yz.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static Vector3 GetWorldPositionOnPlaneX(this Ray ray, Transform t) => GetWorldPositionOnPlaneX(ray, t.position.x);

    public static Vector3 GetWorldPositionOnPlaneY(this Ray ray, float y)
    {
        Camera activeCam = Camera.main;
        Vector3 camPos = activeCam.transform.position;
        ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(Vector3.up, new Vector3(camPos.x, y, camPos.z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static Vector3 GetWorldPositionOnPlaneY(this Ray ray, Transform t) => GetWorldPositionOnPlaneY(ray, t.position.y);

    public static Vector3 GetWorldPositionOnPlaneZ(this Ray ray, float z)
    {
        Camera activeCam = Camera.main;
        Vector3 camPos = activeCam.transform.position;
        ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(camPos.x, camPos.y, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static Vector3 GetWorldPositionOnPlaneZ(this Ray ray, Transform t) => GetWorldPositionOnPlaneZ(ray, t.position.z);

    public static Vector3 GetWorlPositionOnXLocal(this Ray ray, Transform t)
    {
        float x = t.position.x;
        Camera activeCam = Camera.main;
        Vector3 camPos = activeCam.transform.position;
        ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(activeCam.transform.right, new Vector3(x, camPos.y, camPos.z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static Vector3 GetWorlPositionOnYLocal(this Ray ray, Transform t)
    {
        float y = t.position.y;
        Camera activeCam = Camera.main;
        Vector3 camPos = activeCam.transform.position;
        ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(activeCam.transform.up, new Vector3(camPos.x, y, camPos.z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static Vector3 GetWorlPositionOnZLocal(this Ray ray, Transform t)
    {
        float z = t.position.z;
        Camera activeCam = Camera.main;
        Vector3 camPos = activeCam.transform.position;
        ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(activeCam.transform.forward, new Vector3(camPos.x, camPos.y, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public class HitInfo
    {
        public bool isHit;
        public Vector3 hitPoint;
        public GameObject go;
    }

    public static HitInfo GetRayInfoToObject(this Ray ray, string _tag, Transform target, LayerMask layer)
    {
        HitInfo hi = new HitInfo();

        RaycastHit hit;
        ray = new Ray(Camera.main.transform.position, target.position - Camera.main.transform.position);

        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            if (hit.collider.CompareTag(_tag))
            {
                hi.isHit = true;
                hi.hitPoint = hit.point;
                hi.go = hit.collider.gameObject;
            }
            else
                hi.isHit = false;
        }

        return hi;
    }

    public static HitInfo GetRayInfoToObject(this Ray ray, string _tag, Transform target) => GetRayInfoToObject(ray, _tag, target, (LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast"))));

    public static HitInfo GetRayInfoToObject(this Ray ray, Transform target) => GetRayInfoToObject(ray, string.Empty, target, (LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast"))));

    public static HitInfo GetRayInfoToObject(this Ray ray, Transform target, LayerMask layer) => GetRayInfoToObject(ray, string.Empty, target, layer);

    public static HitInfo GetRayHitInfo(this Ray ray) => GetRayHitInfo(ray, string.Empty, LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

    public static HitInfo GetRayHitInfo(this Ray ray, string _tag) => GetRayHitInfo(ray, _tag, LayerMask.NameToLayer("Everything") & ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

    public static HitInfo GetRayHitInfo(this Ray ray, string _tag, LayerMask mask)
    {
        RaycastHit hit;
        HitInfo hi = new HitInfo();
        hi.hitPoint = Vector3.zero;
        hi.isHit = false;
        Camera activeCam = Camera.main;

        ray = activeCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.CompareTag(_tag) || string.IsNullOrEmpty(_tag))
            {
                hi.go = hit.collider.gameObject;
                hi.isHit = true;
                hi.hitPoint = hit.point;
            }
        }

        return hi;
    }
}

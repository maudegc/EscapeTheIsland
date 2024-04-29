using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastUtil
{
    public static bool DebugMode { get; set; }
    public static bool VerboseMode { get; set; }

    private static RaycastHit[] _hits = new RaycastHit[5];

    public static bool TesterCollision(Vector3 center, Vector3 direction, float distance, LayerMask layer, float adjustment = 0.2f)
    {
        float distanceTotal = distance + adjustment;

        bool raycastHit = Physics.Raycast(center, direction,
            distanceTotal, layer);

        DrawRay(center, direction * distanceTotal, raycastHit);

        return raycastHit;
    }

    public static GameObject TesterCollisionObjet(Vector3 center, Vector3 direction, float distance, LayerMask layer,
        float adjustment = 0.2f)
    {
        float distanceTotal = distance + adjustment;

        int nbHits = Physics.RaycastNonAlloc(center, direction, _hits, distanceTotal, layer);
        DrawRay(center, direction * distanceTotal, nbHits > 0);

        return nbHits > 0 ? _hits[0].collider.gameObject : null;
    }

    private static void DrawRay(Vector3 center, Vector3 direction, bool isGreen)
    {
        if (DebugMode)
        {
            Color rayColor;
            rayColor = isGreen ? Color.green : Color.red;

            Debug.DrawRay(center, direction, rayColor);
        }
    }
}

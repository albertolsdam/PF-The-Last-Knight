using UnityEngine;
using Saucy.Interfaces;

// Grants XP to all objects in an area (sphere) that has the IXPReceive interface on them.

namespace Saucy.Modules.XP {
  [CreateAssetMenu(menuName = "Saucy/Modules/XP/Grant XP/In an area")]
  public class DataXPGrantInAnArea : DataXPGrant {
    // Layers to check for IXPReceive interface for.
    [SerializeField] protected LayerMask layersToCheckForReceiveXP;

    public override void GrantXP (int _experience, GameObject _granter) {
      // Create an invisiable sphere that returns all colliders inside it (based on layers we check against).
      Collider[] _hitColliders = Physics.OverlapSphere(_granter.transform.position, radius, layersToCheckForReceiveXP);

      for (int i = 0; i < _hitColliders.Length; i++) {
        // Loop through all colliders and get reference to the IXPReceive interface.
        IXPReceive _receiveXP = _hitColliders[i].GetComponentInParent<IXPReceive>();

        if (_receiveXP != null) {
          // If an object has an IXPReceive interface we can grant XP to it, passing along the granter.
          _receiveXP.ReceiveXP(_experience, _granter);
        }
      }
    }
  }
}

using UnityEngine;
using Saucy.Data;

// Grants XP to all objects in a runtime set. The objects add themselves to the list on OnEnable() and remove themselves on OnDisable().

namespace Saucy.Modules.XP {
  [CreateAssetMenu(menuName = "Saucy/Modules/XP/Grant XP/All in a runtime set")]
  public class DataXPGrantAllInRuntimeSet : DataXPGrant {
    // Reference to a runtime set that can receive XP, so we can loop through later.
    [SerializeField] protected CanReceiveXPRuntimeSet canReceiveXPSet;

    public override void GrantXP (int _experience, GameObject _granter) {
      // Loop through the runtime set and grant XP to all receivers. Pass along the granter.
      for (int i = 0; i < canReceiveXPSet.Items.Count; i++) {
        canReceiveXPSet.Items[i].ReceiveXP(_experience, _granter);
      }
    }
  }
}

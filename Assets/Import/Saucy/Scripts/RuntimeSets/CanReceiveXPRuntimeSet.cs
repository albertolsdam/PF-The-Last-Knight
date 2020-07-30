using UnityEngine;
using Saucy.Interfaces;

// Creates a list of objects at runtime that implements IXPReceive interface, which can be used during runtime.
// Unless you serialize it(?, I have no idea how to do that) or have Odin Inspector you cannot view the list because it has a reference to an interface.

namespace Saucy.Data {
  [CreateAssetMenu(menuName = "Saucy/Data/Runtime sets/Can receive XP")]
  public class CanReceiveXPRuntimeSet : RuntimeSet<IXPReceive> { }
}

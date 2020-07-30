using UnityEngine;

// Creates a list of GameObjects at runtime, which can be used during runtime.

namespace Saucy.Data {
  [CreateAssetMenu(menuName = "Saucy/Data/Runtime sets/Game object")]
  public class GameObjectRuntimeSet : RuntimeSet<GameObject> { }
}

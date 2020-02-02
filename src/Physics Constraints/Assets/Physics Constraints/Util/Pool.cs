/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System.Collections.Generic;

namespace PhysicsConstraints
{
  public class Pool<T>
  {
    private static List<T> s_pool = new List<T>();

    public static T Get()
    {
      if (s_pool.Count == 0)
        return default(T);

      var obj = s_pool[s_pool.Count - 1];
      s_pool.RemoveAt(s_pool.Count - 1);

      return obj;
    }

    public static void Store(T obj)
    {
      s_pool.Add(obj);
    }
  }
}

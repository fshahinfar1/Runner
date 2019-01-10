using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class ObstacleTemplateFetcher
    {
        private static Dictionary<ObstacleType, GameObject> templateCache 
            = new Dictionary<ObstacleType, GameObject>();

        public static GameObject Fetch(ObstacleType type)
        {
            if (templateCache.ContainsKey(type))
            {
                return templateCache[type];
            }

            GameObject template = Resources.Load<GameObject>("Obstacle/" + type.ToString());
            templateCache.Add(type, template);
            return template;
        }
    }
}
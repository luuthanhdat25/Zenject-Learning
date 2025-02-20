using UnityEngine;

namespace Utils
{
    public static class RepeatUtils
    {
        // => UI
        public static bool SetLocalPositionFromWorldPosition
                (this RectTransform rectTransform,
                Vector3 worldPosition,
                RectTransform rectTransformParent,
                RenderMode cameraRenderMode,
                Camera camera = null)
        {
            var screenPosition = camera.WorldToScreenPoint(worldPosition);

            Vector2 localPosition;
            bool isHit = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransformParent,
                screenPosition,
                cameraRenderMode == RenderMode.ScreenSpaceOverlay ? null : camera,
                out localPosition
            );

            if (!isHit) return false;
            rectTransform.localPosition = localPosition;
            return true;
        }

        // => Load Component
        public static T LoadComponent<T>(ref T component, GameObject gameObj) where T : Component
        {
            if (component != null) return component;

            component = gameObj.GetComponent<T>();
            if (component != null) return component;

            Debug.LogError($"Component [{typeof(T).Name}] doesn't have in GameObject [{gameObj.name}]");
            return null;
        }

        public static T LoadComponentInChild<T>(ref T component, GameObject gameObjParent) where T : Component
        {
            if (component != null) return component;

            component = gameObjParent.GetComponentInChildren<T>();
            if (component != null) return component;

            Debug.LogError($"Component [{typeof(T).Name}] doesn't have in GameObject [{gameObjParent.name}] child");
            return null;
        }

        public static T LoadComponentInParent<T>(ref T component, GameObject gameObj) where T : Component
        {
            if (component != null) return component;

            Transform parent = gameObj.transform.parent;
            while (parent != null)
            {
                T foundComponent = parent.GetComponent<T>();
                if (foundComponent != null)
                {
                    component = foundComponent;
                    return component;
                }
                parent = parent.parent;
            }

            Debug.LogError($"Component [{typeof(T).Name}] not found in parents of GameObject [{gameObj.name}]");
            return null;
        }

        // => Transform 2D
        public static bool MoveToPosition(this Transform transform, Vector2 position, float speed)
        {
            Vector2 currentPos = transform.position;
            float distance = Vector2.Distance(currentPos, position);
            float step = speed * Time.fixedDeltaTime;

            if (distance <= step)
            {
                transform.position = position;
                return true;
            }

            transform.position = Vector2.MoveTowards(currentPos, position, step);
            return false;
        }

        public static void RotateLootAt(this Transform transform, Vector2 targetPosition, float offset = 0)
        {
            Vector2 currentPos = transform.position;
            targetPosition.x = targetPosition.x - currentPos.x;
            targetPosition.y = targetPosition.y - currentPos.y;
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }
    }
}

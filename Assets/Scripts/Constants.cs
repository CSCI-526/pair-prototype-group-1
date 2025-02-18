using UnityEngine;

public static class Constants
{
    public static readonly Vector3 SPAWN_POINT = new Vector3(0, 2, -8);
    public static readonly Vector3 WIN_POINT = new Vector3(0, 0, -13);
    public static readonly Vector3 CAMERA_FOLLOW_OFFSET_NORMAL = new Vector3(0, 3, -7);
    public static readonly Vector3 CAMERA_FOLLOW_OFFSET_REVERSE = new Vector3(0, 3, -25);

    public static readonly int MAX_TRACES = 10;
    public static class Forces
    {
        public static readonly float GRAVITY = -9.8f;

    }
    public static class Lanes
    {
        public static readonly float LANE_DISTANCE = 2.45f;
        public static readonly int CENTER = 0;
        public static readonly int LEFT = -1;
        public static readonly int RIGHT = 1;
    }
}

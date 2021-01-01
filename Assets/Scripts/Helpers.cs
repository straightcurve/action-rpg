public static class Helpers {
    public static float LinearMap(float value, float x1, float x2, float y1, float y2) {
        return (value - x1) * (y2 - y1) / (x2 - x1) + y1;
    }
}
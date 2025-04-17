using UnityEngine;
 
public class Draw : MonoBehaviour {
    public Camera cam;
 
    // Map dimensions
    public int totalXPixels = 512;
    public int totalYPixels = 512;
 
    public int brushSize = 4;
    public Color brushColor;
    public Color mapColor;
 
    // Whether the drawing system will use interpolation to make smoother lines
    public bool useInterpolation = true;
 
    // Points on our drawable area
    public Transform topLeftCorner;
    public Transform bottomRightCorner;
    public Transform point;
 
    public Material material;
    public Texture2D generatedTexture;
 
    // The array which contains the color of the pixels
    Color[] colorMap;
 
    // The current coordinates of the cursor in the current frame
    int xPixel = 0;
    int yPixel = 0;
 
    // Variables necessary for interpolation
    bool pressedLastFrame = false;  // This bool remembers whether we clicked over the drawable area in the last frame
    int lastX = 0;                  // These variables remember the coordinates of the cursor in the last frame
    int lastY = 0;
 
    // These variables hold constants which are precalculated in order to save performance
    float xMult;
    float yMult;
 
    private void Start()
    {
        // Initializing the colorMap array with width * height elements
        colorMap = new Color[totalXPixels * totalYPixels];
        generatedTexture = new Texture2D(totalYPixels, totalXPixels, TextureFormat.RGBA32, false); //Generating a new texture with width and height
        generatedTexture.filterMode = FilterMode.Point;
        material.SetTexture("_BaseMap", generatedTexture); //Giving our material the new texture
 
        ResetColor();
 
        xMult = totalXPixels / (bottomRightCorner.localPosition.x - topLeftCorner.localPosition.x);
        yMult = totalYPixels / (bottomRightCorner.localPosition.y - topLeftCorner.localPosition.y);
    }
 
    private void Update()
    {
        if (Input.GetMouseButton(0))
            CalculatePixel();
        else
            pressedLastFrame = false;
    }
 
    void CalculatePixel()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            point.position = hit.point;
            xPixel = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult);
            yPixel = (int)((point.localPosition.y - topLeftCorner.localPosition.y) * yMult);
            ChangePixelsAroundPoint();
        }
        else
            pressedLastFrame = false;
    }
 
    void ChangePixelsAroundPoint() // This function checks whether interpolation should be applied and if it should, it applies it
    {
        if(useInterpolation && pressedLastFrame && (lastX != xPixel || lastY != yPixel)) // Check if we should use interpolation
        {
            int dist = (int)Mathf.Sqrt((xPixel - lastX) * (xPixel - lastX) + (yPixel - lastY) * (yPixel - lastY)); // Calculate the distance between the current pixel and the pixel from last frame
            for (int i = 1; i <= dist; i++) // Loop through the points on the determined line
                DrawBrush((i * xPixel + (dist - i) * lastX) / dist, (i * yPixel + (dist - i) * lastY) / dist); // Call the DrawBrush method on the determined points
        }
        else // We shouldn't apply interpolation
            DrawBrush(xPixel, yPixel); // Call the DrawBrush method
        pressedLastFrame = true; // We should apply interpolation on the next frame
        lastX = xPixel;
        lastY = yPixel;
        SetTexture();
    }
 
    void DrawBrush(int xPix, int yPix) // This function takes a point on the canvas as a parameter and draws a circle with radius brushSize around it
    {
        int i = xPix - brushSize + 1, j = yPix - brushSize + 1, maxi = xPix + brushSize - 1, maxj = yPix + brushSize - 1; // Declaring the limits of the circle
        if (i < 0) // If either lower boundary is less than zero, set it to be zero
            i = 0;
        if (j < 0)
            j = 0;
        if (maxi >= totalXPixels) // If either upper boundary is more than the maximum amount of pixels, set it to be under
            maxi = totalXPixels - 1;
        if (maxj >= totalYPixels)
            maxj = totalYPixels - 1;
        for(int x=i; x<=maxi; x++)// Loop through all of the points on the square that frames the circle of radius brushSize
        {
            for(int y=j; y<=maxj; y++)
            {
                if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize) // Using the circle's formula(x^2+y^2<=r^2) we check if the current point is inside the circle
                    colorMap[x * totalYPixels + y] = brushColor;
            }
        }
    }
 
    void SetTexture()
    {
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }
 
    void ResetColor()
    {
        for (int i = 0; i < colorMap.Length; i++)
            colorMap[i] = mapColor;
        SetTexture();
    }
 
}
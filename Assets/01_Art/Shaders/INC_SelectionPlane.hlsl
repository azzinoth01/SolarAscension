void SquareSDF_float(float3 absWPos, float halfLength, out float Mask)
{
    float3 componentwiseEdgeDistance = abs(absWPos) - halfLength;
    float outsideSdf = length(max(componentwiseEdgeDistance, 0));
    float insideSdf = min(max(componentwiseEdgeDistance.x, max(componentwiseEdgeDistance.y, componentwiseEdgeDistance.z)), 0);
    Mask = outsideSdf + insideSdf;
}

void SquareSDF_half(half3 absWPos, half halfLength, out half Mask)
{
    half3 componentwiseEdgeDistance = abs(absWPos) - halfLength;
    half outsideSdf = length(max(componentwiseEdgeDistance, 0));
    half insideSdf = min(max(componentwiseEdgeDistance.x, max(componentwiseEdgeDistance.y, componentwiseEdgeDistance.z)), 0);
    Mask = outsideSdf + insideSdf;
}

void GenerateCells_float(float3 absWPos, float3 period, out float3 cellWPos)
{
    absWPos = fmod(absWPos, period);
    absWPos += period;
    cellWPos = fmod(absWPos, period);
}

void GenerateCells_half(half3 absWPos, half3 period, out half3 cellWPos)
{
    absWPos = fmod(absWPos, period);
    absWPos += period;
    cellWPos = fmod(absWPos, period);
}
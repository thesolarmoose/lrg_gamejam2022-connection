using System.Collections.Generic;
using UnityEngine;

/*
 * Code downloaded from https://github.com/dci05049/SlingShotVerletIntegration/blob/master/Assets/RopeBridge.cs
 * Modified a bit by https://github.com/mnicolas94
 */
public class Rope : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    
    [SerializeField] private float _ropeSegLen = 0.25f;
    [SerializeField] private int _segmentLength = 35;
    [SerializeField] private float _lineWidth = 0.1f;

    [SerializeField] private Vector2 _gravityForce;
    [SerializeField] private float _drag;
    
    private LineRenderer _lineRenderer;
    private List<RopeSegment> _ropeSegments = new List<RopeSegment>();

    // Use this for initialization
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = _startPoint.position;

        for (int i = 0; i < _segmentLength; i++)
        {
            _ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= _ropeSegLen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        for (int i = 1; i < _segmentLength; i++)
        {
            RopeSegment firstSegment = _ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            velocity -= velocity * _drag;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += _gravityForce * Time.fixedDeltaTime;
            _ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constraint to First Point 
        RopeSegment firstSegment = _ropeSegments[0];
        firstSegment.posNow = _startPoint.position;
        _ropeSegments[0] = firstSegment;

        //Constraint to Last Point 
        RopeSegment endSegment = _ropeSegments[_ropeSegments.Count - 1];
        endSegment.posNow = _endPoint.position;
        _ropeSegments[_ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < _segmentLength - 1; i++)
        {
            RopeSegment firstSeg = _ropeSegments[i];
            RopeSegment secondSeg = _ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - _ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > _ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < _ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                _ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                _ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                _ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;

        Vector3[] ropePositions = new Vector3[_segmentLength];
        for (int i = 0; i < _segmentLength; i++)
        {
            ropePositions[i] = _ropeSegments[i].posNow;
        }

        _lineRenderer.positionCount = ropePositions.Length;
        _lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
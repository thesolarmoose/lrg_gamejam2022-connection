using System.Collections.Generic;
using UnityEngine;

/*
 * Code downloaded from https://github.com/dci05049/SlingShotVerletIntegration/blob/master/Assets/RopeBridge.cs
 * Modified a bit by https://github.com/mnicolas94
 */
namespace Hook
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private RopeTip _startPoint;
        [SerializeField] private RopeTip _endPoint;
    
        [SerializeField] private float _ropeSegLen = 0.25f;
        [SerializeField] private int _segmentLength = 35;
        [SerializeField] private float _lineWidth = 0.1f;

        [SerializeField] private Vector2 _gravityForce;
        [SerializeField, Range(0.0f, 1.0f)] private float _drag;
        [SerializeField] private int _constraintsIters;
    
        private LineRenderer _lineRenderer;
        private List<RopeSegment> _ropeSegments = new List<RopeSegment>();
        private bool _initialized;

        public float Length
        {
            get => _ropeSegLen * _segmentLength;
            set => _ropeSegLen = value / _segmentLength;
        }

        public RopeTip StartPoint
        {
            get => _startPoint;
            set => _startPoint = value;
        }

        public RopeTip EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }

        private void Start()
        {
            Initialize(_startPoint.position);
        }

        public void Initialize(Vector2 position)
        {
            if (!_initialized)
            {
                _initialized = true;
                _lineRenderer = GetComponent<LineRenderer>();

                for (int i = 0; i < _segmentLength; i++)
                {
                    _ropeSegments.Add(new RopeSegment(position));
                }
            }
        }

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
            for (int i = 0; i < _segmentLength; i++)
            {
                RopeSegment segment = _ropeSegments[i];
                Vector2 velocity = segment.posNow - segment.posOld;
                velocity *= 1 - _drag;
                segment.posOld = segment.posNow;
                segment.posNow += velocity;
                segment.posNow += _gravityForce * Time.fixedDeltaTime;
                _ropeSegments[i] = segment;
            }

            //CONSTRAINTS
            for (int i = 0; i < _constraintsIters; i++)
            {
                ApplyConstraint();
            }
        }

        private void ApplyConstraint()
        {
            //Constraint to First Point 
            RopeSegment firstSegment = _ropeSegments[0];
            var firstPos = Vector2.Lerp(firstSegment.posNow, _startPoint.position, _startPoint.Weight);
            firstSegment.posNow = firstPos;
            _startPoint.position = firstPos;
//        firstSegment.posNow = _startPoint.position;
            _ropeSegments[0] = firstSegment;

            //Constraint to Last Point 
            RopeSegment endSegment = _ropeSegments[_ropeSegments.Count - 1];
            var endPos = Vector2.Lerp(endSegment.posNow, _endPoint.position, _endPoint.Weight);
            endSegment.posNow = endPos;
            _endPoint.position = endPos;
//        endSegment.posNow = _endPoint.position;
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
                firstSeg.posNow -= changeAmount * 0.5f;
                _ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                _ropeSegments[i + 1] = secondSeg;
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
}
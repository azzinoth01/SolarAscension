using UnityEngine;

namespace SolarAscension {
	public class InputAxisConverter {
		private readonly float _smoothSpeed;
		private Vector2 _smoothingVelocity, _output;

		public InputAxisConverter(float smoothSpeed) {
			_smoothSpeed = smoothSpeed;
		}
		
		public Vector2 InputToAxis(Vector2 input) {
			_output = Vector2.SmoothDamp(_output, input, ref _smoothingVelocity, _smoothSpeed);

			if ( Mathf.Abs(_output.x) < 0.001f ) { _output.x = 0f; }
			if ( Mathf.Abs(_output.x) > 0.999f ) { _output.x = 1f * Mathf.Sign(_output.x); }
			if ( Mathf.Abs(_output.y) < 0.001f ) { _output.y = 0f; }
			if ( Mathf.Abs(_output.y) > 0.999f ) { _output.y = 1f * Mathf.Sign(_output.y); }

			return _output;
		}
	}
}
using UnityEngine;
using UnityEngine.UI;

namespace CrusaderUI.Scripts
{
	public class HPFlowController : MonoBehaviour {
	
		private Material _material;

		// Start 대신 Awake를 사용하여 초기화 시점을 앞당깁니다.
		private void Awake ()
		{
			Initialize();
		}

		private void Initialize()
		{
			if (_material == null)
			{
				var img = GetComponent<Image>();
				if (img != null)
				{
					_material = img.material;
				}
			}
		}

		public void SetValue(float value)
		{
			// 혹시라도 초기화가 안 되어 있다면 여기서 한 번 더 시도합니다. (방어 코드)
			if (_material == null) Initialize();

			if (_material != null)
			{
				_material.SetFloat("_FillLevel", value);
			}
		}
	}
}

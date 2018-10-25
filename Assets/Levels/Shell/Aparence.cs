using UnityEngine;

[CreateAssetMenu(fileName="Aparence", menuName="Shell/Aparence", order=1)]
public class Aparence : ScriptableObject {
	[Header("Colors")]
	public Color borderColor = new Color(173/255f,169/255f,165/255f);
	public Color backgroundColor = new Color(48/255f,10/255f,36/255f);
	public Color textColor = new Color(1f,1f,1f);
	[Header("Sprites")]
	public Sprite topCornerLeft;
	public Sprite topCornerRight;
	public Sprite topCenter;
	public Sprite border;
	[Header("Font")]
	public Font font;
}

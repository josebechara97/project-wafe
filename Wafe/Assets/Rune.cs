using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rune : MonoBehaviour
{
    public Image borderImage;
    public Image elementImage;
    public Image shapeImage;
    public Image behaviorImage;

    [Header("Spell Composition")]
    public Spell.Behavior behavior;
    public Sprite[] behaviorSprites;
    public Spell.Element element;
    public Sprite[] elementSprites;
    public Color[] elementColors;
    public Spell.Shape shape;
    public Sprite[] shapeSprites;
    public KeyCode fire;
    public GameObject spellPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        borderImage.color = elementColors[(int)element];

        behaviorImage.sprite = behaviorSprites[(int)behavior];
        behaviorImage.color = elementColors[(int)element];

        elementImage.sprite = elementSprites[(int)element];
        elementImage.color = elementColors[(int)element];

        shapeImage.sprite = shapeSprites[(int)shape];
        shapeImage.color = elementColors[(int)element];

        if (Input.GetKeyDown(fire))
        {
            GameObject spell = Instantiate(spellPrefab, transform.position, transform.rotation) as GameObject;
            Spell spellProperties = spell.GetComponent<Spell>();
            spellProperties.caster = gameObject;
            spellProperties.behavior = this.behavior;
            spellProperties.element = this.element;
            spellProperties.shape = this.shape;
        }
    }
}

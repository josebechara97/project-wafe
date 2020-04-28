using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public enum Behavior
    {
        Projectable,
        Placeable,
        Wearable
    }
    [Header("Spell Composition")]
    public Behavior behavior;
    public enum Element
    {
        Water,
        Air,
        Fire,
        Earth
    }
    public Element element;
    public enum Shape
    {
        Sphere,
        Shield,
        Column,
        Wall
    }
    public Shape shape;
    [Header("Final Spell Properties")]
    public float finalSpeedModifier = 1;
    public float finalEnduranceModifier = 1;
    public float finalPushModifier = 1;
    public float finalDamageModifier = 1;
    public float finalWeightModifier = 1;

    [Header("Public Referrences")]
    public GameObject caster;
    public GameObject fire;
    public GameObject[] elements;
    public GameObject[] shapes;

    [Header("Final Spell Properties")]
    public float avgSpeed = 10;
    public float avgEndurance = 1;
    public float avgPush = 1;
    public float avgDamage = 1;
    public float avgWeight = 1;

    [Header("Testing Data")]
    private Renderer currentRenderer;
    private Vector3 originalScale;
    private float fireSizeModifier = 1;
    private float speedModifierForScaling = 0.5f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {

        ActivateRenderer(shape);

        switch (element)
        {
            case Element.Water:
                UpdateIfWater();
                break;
            case Element.Air:
                UpdateIfAir();
                break;
            case Element.Fire:
                UpdateIfFire();
                break;
            case Element.Earth:
                UpdateIfEarth();
                break;
        }
        switch (behavior)
        {
            case Behavior.Projectable:
                UpdateIfProjectable();
                break;
            case Behavior.Placeable:
                UpdateIfPlaceable();
                break;
            case Behavior.Wearable:
                UpdateIfWearable();
                break;
        }
    }

    private void UpdateIfEarth()
    {
        currentRenderer.material = elements[(int)Element.Earth].GetComponent<Renderer>().sharedMaterial;
        fire.SetActive(false);
    }

    private void UpdateIfFire()
    {
        currentRenderer.material = elements[(int)Element.Fire].GetComponent<Renderer>().sharedMaterial;
        //transform.Rotate(Vector3.up * 1000 * Time.deltaTime);
        fire.SetActive(true);
    }

    private void UpdateIfAir()
    {
        currentRenderer.material = elements[(int)Element.Air].GetComponent<Renderer>().sharedMaterial;
        transform.Rotate(Vector3.up * 1000 * Time.deltaTime);
        fire.SetActive(false);
    }

    private void UpdateIfWater()
    {
        currentRenderer.material = elements[(int)Element.Water].GetComponent<Renderer>().sharedMaterial;
        fire.SetActive(false);
    }

    private void ActivateRenderer(Shape shape)
    {
        int i = 0;
        foreach (GameObject currentRenderer in shapes)
        {

            if (i == (int)shape)
            {
                this.currentRenderer = currentRenderer.GetComponent<Renderer>();
                if (element == Element.Fire)
                    currentRenderer.SetActive(false);
                else currentRenderer.SetActive(true);
                fire.transform.localScale = currentRenderer.gameObject.transform.localScale * fireSizeModifier;
            }
            else
            {
                currentRenderer.SetActive(false);
            }
            i++;
        }
    }

    void UpdateIfProjectable()
    {
        transform.Translate(Vector3.forward * this.FinalSpeed() * avgSpeed * Time.deltaTime);
    }



    private bool placed;
    private float targetY;
    private bool targetYReady;
    private float t = 0.0f;
    void UpdateIfPlaceable()
    {
        if (!placed)
        {
            currentRenderer.gameObject.SetActive(false);
            transform.localScale = Vector3.one * 0.1f;
            transform.Translate(Vector3.forward * finalSpeedModifier * Time.deltaTime);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (!targetYReady)
            {
                targetY = transform.localScale.y;
                targetYReady = true;
            }
            else
            {
                Vector3 scale = transform.localScale;
                t += Time.deltaTime * (finalSpeedModifier * speedModifierForScaling);
                scale.y = Mathf.Lerp(0, targetY, t);
                transform.localScale = scale;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                if (element != Element.Fire)
                    currentRenderer.gameObject.SetActive(true);
            }
        }
    }

    void UpdateIfWearable()
    {
        transform.parent = caster.transform;
        //transform.RotateAround(caster.transform.position, Vector3.up, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.isStatic && this.behavior == Behavior.Placeable)
        {
            placed = true;
            transform.localScale = originalScale;
            transform.parent = other.gameObject.transform;
        }
    }

    private float FinalSpeed()
    {
        float shapeSpeedModifier =  shapes[(int)shape].GetComponent<SpellPropertiesModifier>().finalSpeedMultiplier;
        float elementSpeedModifier = elements[(int)element].GetComponent<SpellPropertiesModifier>().finalSpeedMultiplier;
        this.finalSpeedModifier = (shapeSpeedModifier + elementSpeedModifier) / 2;
        return finalSpeedModifier;
    }

}

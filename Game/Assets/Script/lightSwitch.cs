using UnityEngine;
using UnityEngine.UI;

public class lightSwitch : MonoBehaviour
{
    public GameObject intText, lightObject;
    public bool toggle = true, interactable;
    public Renderer lightBulb;
    public Material offlight, onlight;
    public AudioSource lightSwitchSound;
    public Animator switchAnim;
    public Button interactButton; // Reference to your UI button

    private void SetVisibility(bool visible)
    {
        intText.SetActive(visible);
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(visible);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            SetVisibility(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            SetVisibility(false);
            interactable = false;
        }
    }

    private void Update()
    {
        // Check for interaction using the specified key
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            TryToggleLightSwitch();
        }

        // Update light switch state
        if (toggle == false)
        {
            lightObject.SetActive(false);
            lightBulb.material = offlight;
        }
        else
        {
            lightObject.SetActive(true);
            lightBulb.material = onlight;
        }
    }

    private void TryToggleLightSwitch()
    {
        toggle = !toggle;
        switchAnim.ResetTrigger("press");
        switchAnim.SetTrigger("press");
    }

    // Method to handle UI button click
    public void OnInteractButtonClick()
    {
        TryToggleLightSwitch();
    }
}

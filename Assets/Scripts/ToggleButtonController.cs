using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonController : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public Button buttonTrue; // Reference to the first button
    public Button buttonFalse; // Reference to the second button

    // Sprites for buttonTrue
    public Sprite buttonTrueSelectedSprite;
    public Sprite buttonTrueDeselectedSprite;

    // Sprites for buttonFalse
    public Sprite buttonFalseSelectedSprite;
    public Sprite buttonFalseDeselectedSprite;

    private bool shouldFlock; // The bool controlled by the buttons

    void Start()
    {
        // Initialize the buttons with listeners
        buttonTrue.onClick.AddListener(() => SetFlockedState(true));
        buttonFalse.onClick.AddListener(() => SetFlockedState(false));

        // Set the initial state
        SetFlockedState(false); // Default state: false
    }

    private void SetFlockedState(bool state)
    {
        shouldFlock = state;

        // Update the player's shouldFlocked property
        if (player.TryGetComponent(out PlayerController playerController)) // Assuming a PlayerController script
        {
            playerController.shouldFlock = shouldFlock;
        }

        // Update button sprites
        buttonTrue.image.sprite = state ? buttonTrueSelectedSprite : buttonTrueDeselectedSprite;
        buttonFalse.image.sprite = !state ? buttonFalseSelectedSprite : buttonFalseDeselectedSprite;
    }
}

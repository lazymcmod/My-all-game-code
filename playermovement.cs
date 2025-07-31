using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector me set karne ke liye variables
    public CharacterController controller; // Player ka CharacterController component
    public float speed = 12f;              // Player ki movement speed
    public float gravity = -19.62f;        // Gravity ki taakat
    public float jumpHeight = 3f;          // Jump ki unchai

    // Zameen check karne ke liye variables
    private Vector3 velocity;
    private bool isGrounded;

    // Har frame me yeh function chalta hai
    void Update()
    {
        // Check karna ki player zameen par hai ya nahi
        isGrounded = controller.isGrounded;

        // Agar player zameen par hai aur uski vertical velocity hai, to use reset karna
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Player ko zameen par rakhta hai
        }

        // --- Movement ---
        // Keyboard se W,A,S,D input lena
        float x = Input.GetAxis("Horizontal"); // A aur D ke liye
        float z = Input.GetAxis("Vertical");   // W aur S ke liye

        // Movement ki direction banana
        Vector3 move = transform.right * x + transform.forward * z;

        // Player ko uss direction me move karna
        controller.Move(move * speed * Time.deltaTime);


        // --- Jump ---
        // "Jump" button (default me Spacebar) dabane par check karna
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Jump ke liye zaroori velocity calculate karna
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        // --- Gravity ---
        // Har second gravity ko velocity me jodna
        velocity.y += gravity * Time.deltaTime;

        // Player par gravity ka asar daalna
        controller.Move(velocity * Time.deltaTime);
    }
}
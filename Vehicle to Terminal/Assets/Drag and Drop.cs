using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 initialPosition;
    private Terminal terminal;
    public Jeep jeepScript; // Reference to the Jeep script
    public Pedicab pedicabScript;
    public Motorcycle motorcycleScript;
    public Bus busScript;
    private Vector3 originalPosition;
    // Add similar references for the other vehicle types

    private void Start()
    {
        terminal = GameObject.Find("Terminal").GetComponent<Terminal>();
    }

    private void OnMouseDown()
    {
        // Check if the vehicle count is greater than 0 before allowing dragging
        if (GetVehicleCount() > 0)
        {
            isDragging = true;
            originalPosition = transform.position; // Store the original position
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (Vector3.Distance(transform.position, terminal.transform.position) < 1.0f)
        {
            // Handle dropping the vehicle on the terminal
            if (terminal.currentColor == Color.white)
            {
                terminal.ChangeColor(GetComponent<SpriteRenderer>().color);
                terminal.IncrementCount();
                UpdateVehicleCount(-1); // Decrease the count by 1 when a vehicle is first dropped
            }
            else if (terminal.currentColor == GetComponent<SpriteRenderer>().color)
            {
                if (GetVehicleCount() > 0)
                {
                    terminal.IncrementCount();
                    DecrementVehicleCount();

                    // Disable the renderer of the object when it's dropped onto the terminal
                    GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    // Vehicle count is 0; do not increment, but allow moving
                    ReturnToOriginalPosition();
                    return;
                }
            }
            else
            {
                terminal.ChangeColor(GetComponent<SpriteRenderer>().color);
                terminal.ResetCount(1); // Reset the count to 1 when a different vehicle is dropped
                DecrementVehicleCount();
            }

            // Reset the position to the original position
            transform.position = originalPosition;

            // Enable the renderer after moving the vehicle back to its original position
            GetComponent<Renderer>().enabled = true;

            // Print the available vehicle count
            PrintAvailableVehicleCount();
        }
        else
        {
            // The vehicle was not dropped on the terminal, move it back to the original position
            ReturnToOriginalPosition();
        }
    }

    private void PrintAvailableVehicleCount() //Displays available vehicle
    {
        if (jeepScript != null)
        {
            Debug.Log("Available Jeep Count: " + jeepScript.vehicleCount);
        }
        if (pedicabScript != null)
        {
            Debug.Log("Available Pedicab Count: " + pedicabScript.vehicleCount);
        }
        if (motorcycleScript != null)
        {
            Debug.Log("Available Motorcycle Count: " + motorcycleScript.vehicleCount);
        }
        if (busScript != null)
        {
            Debug.Log("Available Bus Count: " + busScript.vehicleCount);
        }
    }

    private void ReturnToOriginalPosition()
    {
        // Move the vehicle back to its original position
        transform.position = originalPosition;
    }

    private void DecrementVehicleCount() //limited count of vehicle
    {
        if (gameObject.CompareTag("Jeep") && jeepScript != null)
        {
            jeepScript.DecrementCount();
        }
        else if (gameObject.CompareTag("Pedicab") && pedicabScript != null)
        {
            pedicabScript.DecrementCount();
        }
        else if (gameObject.CompareTag("Motorcycle") && motorcycleScript != null)
        {
            motorcycleScript.DecrementCount();
        }
        else if (gameObject.CompareTag("Bus") && busScript != null)
        {
            busScript.DecrementCount();
        }
    }

    private int GetVehicleCount()
    {
        if (gameObject.CompareTag("Jeep") && jeepScript != null)
        {
            return jeepScript.vehicleCount;
        }
        else if (gameObject.CompareTag("Pedicab") && pedicabScript != null)
        {
            return pedicabScript.vehicleCount;
        }
        else if (gameObject.CompareTag("Motorcycle") && motorcycleScript != null)
        {
            return motorcycleScript.vehicleCount;
        }
        else if (gameObject.CompareTag("Bus") && busScript != null)
        {
            return busScript.vehicleCount;
        }

        // Default to 0 if no matching conditions are met
        return 0;
    }

    private void UpdateVehicleCount(int count)
    {
        if (gameObject.CompareTag("Jeep") && jeepScript != null)
        {
            int previousCount = jeepScript.vehicleCount;
            jeepScript.vehicleCount += count;
        }
        else if (gameObject.CompareTag("Pedicab") && pedicabScript != null)
        {
            int previousCount = pedicabScript.vehicleCount;
            pedicabScript.vehicleCount += count;
        }
        else if (gameObject.CompareTag("Motorcycle") && motorcycleScript != null)
        {
            int previousCount = motorcycleScript.vehicleCount;
            motorcycleScript.vehicleCount += count;
        }
        else if (gameObject.CompareTag("Bus") && busScript != null)
        {
            int previousCount = busScript.vehicleCount;
            busScript.vehicleCount += count;
        }
    }
}

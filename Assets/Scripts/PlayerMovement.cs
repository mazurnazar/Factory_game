using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] GameObject player;
    [SerializeField] private float speed =1;

    [SerializeField] Image joystickContainer, joystick;
    [SerializeField] Canvas canvas;

    private Vector3 direction; // direction of movement
    private float angle; // angle of movement
    private Vector3 mousePos; // position of mouse

    // when clicked enable joystick and set position of ckick
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                                                                Input.mousePosition,
                                                                canvas.worldCamera,
                                                                out pos);

        mousePos = Input.mousePosition; 
        mousePos.z = 0;
        joystickContainer.gameObject.SetActive(true);
        joystickContainer.GetComponent<RectTransform>().position = canvas.transform.TransformPoint(pos);
    }

    // when dragging -> move middle circle of joystick in boundaries of joystic container
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickContainer.transform as RectTransform, 
                                                                Input.mousePosition, 
                                                                canvas.worldCamera, 
                                                                out pos);
        float x = pos.x / joystickContainer.rectTransform.sizeDelta.x;
        float y = pos.y / joystickContainer.rectTransform.sizeDelta.y;

        // set direction
        direction = new Vector3(x, y, 0);
        direction = direction.magnitude > 1 ? direction.normalized : direction;
        joystick.rectTransform.anchoredPosition = new Vector2(direction.x * joystickContainer.rectTransform.sizeDelta.x / 3,
                                                              direction.y * joystickContainer.rectTransform.sizeDelta.y / 3);
        
        angle = Vector3.Angle(Vector3.up, direction); // set angle of direction
        if (direction.x < 0) angle *= -1;
    }

    // on click up deactivate joystick and set direction to zero
    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.rectTransform.anchoredPosition = Vector2.zero;
        direction = Vector3.zero;
        joystickContainer.gameObject.SetActive(false);
    }

    void Start()
    {
        direction = Vector3.zero;
    }
    void Update()
    {
        // change position and rotation of player according to direction of movement
        player.transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * speed; 
        player.transform.eulerAngles = new Vector3(0, angle, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        direction = Vector3.zero;
    }
    private void OnCollisionExit(Collision collision)
    {
       
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Amazon : MonoBehaviour
{
    public List<Sprite> itemImages;
    public List<string> itemNames;
    public List<string> itemPrices;

    //Amazon App Ui Components
    public Image _itemImage;
    public TextMeshProUGUI _itemName;
    public TextMeshProUGUI _itemPrice;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        _itemImage.sprite = itemImages[0];
        _itemName.text = itemNames[0];
        _itemPrice.text = itemPrices[0];
    }

    public void NextItem()
    {
        index++;
        if(index >= itemImages.Count)
        {
            index = 0;
        }
        UpdateDisplay();
    }

    public void PreviousItem()
    {
        index--;
        if(index < 0)
        {
            index = itemImages.Count - 1;
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        _itemImage.sprite = itemImages[index];
        _itemName.text = itemNames[index];
        _itemPrice.text = itemPrices[index];
    }
}

using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
     public GameObject[] skins;
     public int selectedCharacter;

     public GameObject CharacterPrefab;

     private void Awake()
     {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject player in skins)
            player.SetActive(false);

        skins[selectedCharacter].SetActive(true);
     }

     public void ChangeNext()
     {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if(selectedCharacter == skins.Length)
            selectedCharacter=0;

        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
     }

     public void ChangePrevious()
     {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter == -1)
            selectedCharacter=skins.Length -1;

        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
     }

     public void OnSelectCharacter()
     {
        GameManager.Instance.SelectCharacter(CharacterPrefab);
        Debug.Log(CharacterPrefab.name + " Selected!");
     }
}

using TMPro;
using UnityEngine;

public class UpgradeButton : UIButton
{
    [SerializeField] private TMP_Text _captionText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _adText;

    public TMP_Text CaptionText => _captionText;
    public TMP_Text LevelText => _levelText;
    public TMP_Text PriceText => _priceText;
    public TMP_Text AdText => _adText;
}

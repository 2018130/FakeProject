using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // 파일 입출력을 위해 필수

public class PhoneGalleryManager : MonoBehaviour
{
    [Header("Capture Settings")]
    public Camera photoCamera; // 촬영할 카메라 (RenderTexture가 연결된 카메라)
    public RenderTexture targetTexture; // 위 카메라에 연결된 렌더 텍스처

    [Header("Gallery UI")]
    public GameObject galleryPanel; // 갤러리 전체 패널 (켜고 끌 것)
    public Transform contentParent; // ScrollView의 Content 오브젝트
    public GameObject titlePrefab;
    public GameObject UpperbarPrefab;
    public GameObject photoPrefab;  // 갤러리에 추가될 사진 프리팹 (Image 컴포넌트가 있어야 함)
    public Animator openAnimator;

    private string savePath;

    [SerializeField]
    public PhotoSystem photoSystem;

    void Start()
    {
        // PC와 모바일 모두에서 접근 가능한 저장 경로 설정
        savePath = Path.Combine(Application.persistentDataPath, "MyGamePhotos");

        // 폴더가 없으면 생성
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        else
        {
            Directory.Delete(savePath, true);
            Directory.CreateDirectory(savePath);
        }
        photoSystem = FindAnyObjectByType<PhotoSystem>();
    }

    public void Initialize(Camera camera)
    {
        photoCamera = camera;
    }

    // [기능 1] 사진 촬영 및 저장 (기존 코드에서 특정 키나 버튼을 누르면 이 함수를 호출하세요)
   
    public void CaptureAndSave()
    {
        StartCoroutine(CaptureRoutine());
        photoSystem.TakePhoto();
    }

    IEnumerator CaptureRoutine()
    {
        // 1. 현재 렌더 텍스처 활성화
        RenderTexture.active = targetTexture;

        // 2. 텍스처 생성 및 픽셀 읽기
        Texture2D texture = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
        texture.Apply();

        // 3. 렌더 텍스처 해제 (기존 렌더링 복구)
        RenderTexture.active = null;

        // 4. 파일로 저장 (날짜시간을 이름으로)
        byte[] bytes = texture.EncodeToPNG();
        string fileName = "Photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(savePath, fileName);

        File.WriteAllBytes(filePath, bytes);
        Debug.Log("사진 저장 완료: " + filePath);

        // 메모리 정리
        Destroy(texture);

        yield return null;
    }

    // [기능 2] 갤러리 열기 (저장된 사진 불러오기)
    public void OpenGallery()
    {
        galleryPanel.SetActive(true);

        openAnimator.SetTrigger("Opened");
        LoadPhotos();
    }

    public void CloseGallery()
    {
        // 애니메이션만 실행
        openAnimator.SetTrigger("Closed");
    }


    void LoadPhotos()
    {
        // 1. 기존에 띄워진 사진들 제거 (중복 방지)
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 2. 저장 경로에서 모든 png 파일 가져오기
        string[] files = Directory.GetFiles(savePath, "*.png");

        RectTransform titleRect = Instantiate(titlePrefab, contentParent).GetComponent<RectTransform>();
        RectTransform upperRect = Instantiate(UpperbarPrefab, contentParent).GetComponent<RectTransform>();
        float height = titleRect.sizeDelta.y;
        height += upperRect.sizeDelta.y;

        foreach (string file in files)
        {
            // 3. 파일 읽어서 텍스처로 변환
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2);
            if (tex.LoadImage(bytes)) // 이미지 로드 성공 시
            {
                // 4. Sprite로 변환
                Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                // 5. 프리팹 생성 및 이미지 할당
                GameObject newPhoto = Instantiate(photoPrefab, contentParent);
                newPhoto.GetComponent<Image>().sprite = newSprite;
                height += newPhoto.GetComponent<RectTransform>().sizeDelta.y + 20;
            }
        }

        contentParent.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(contentParent.GetComponent<RectTransform>().sizeDelta.x, height);
    }
}
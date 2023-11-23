// 카드 효과를 관리하는 인터페이스
// 각 카드의 효과는 "Card_00n.cs"와 같은 방식으로 클래스를 만들어 구현한다.
public interface ICard
{
    // 덱에 카드가 추가될 때 발동하는 효과
    void OnAcquire();

    // 카드를 손패에 드로우했을 때 발동하는 효과
    void OnDraw();

    // 손패에 있는 카드를 사용했을 때 발동하는 효과
    void OnUse();

    // 덱에서 카드를 제거할 때 발동하는 효과
    void OnRemove();
}

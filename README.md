# 2D-RPG 动作游戏

一个完整的 2D 横版动作 RPG Demo，使用 Unity 独立开发。包含玩家状态机、敌人 AI、技能树、装备背包、Boss 战、存档系统等完整游戏系统。

## 项目概述

- **引擎**：Unity
- **语言**：C#
- **类型**：2D 横版动作 RPG
- **开发方式**：个人独立开发

## 核心系统

### 架构设计

- Entity 基类 + 组件化架构，所有角色（玩家/敌人/Boss）继承自 Entity
- ISaveManager 接口统一存档管理，各系统独立实现序列化逻辑
- 全局系统通过单例管理：SkillManager、InventoryManager、SaveManager、AudioManager

### 状态机系统

**玩家**：分层状态机，15+ 种状态
- 基础：Idle / Move / Jump / Air / WallSlide / WallJump
- 战斗：PrimaryAttack / CounterAttack / AimSword / CatchSword / BlackHole
- 其他：Dash / Die

**敌人**：5 种独立 AI，每种有完整的行为状态（巡逻/追击/攻击/眩晕/死亡）
- Skeleton（骷髅）
- Slime（史莱姆）
- Archer（弓箭手）
- Shady（暗影）
- DeathBriner（Boss）

### 战斗系统

- 15 种属性（力量/敏捷/智力/暴击率/暴击伤害/护甲/闪避/魔抗/3 种元素伤害等）
- 伤害计算包含闪避、暴击、护甲减免
- 3 种元素异常状态：点燃（DoT）、冰冻（减甲）、感电（链式传导）

### 技能系统

7 种技能，支持树状解锁（货币消耗 + 前置技能校验 + 互斥分支）：
- Dash（冲刺）/ Clone（分身）/ Sword（飞剑，4 种模式）/ Crystal（水晶）
- BlackHole（黑洞）/ Parry（格挡）/ Dodge（闪避）
- UI_SkillTreeSlot 实现依赖验证和解锁逻辑

### 装备背包系统

- 装备/材料分仓管理，Dictionary 快速查找
- 穿戴属性加成（AddModifiers）
- 物品合成系统，ScriptableObject 数据驱动
- 支持 Tooltip 显示装备详情

### Boss 战

- DeathBriner：6 阶段 AI（空闲 → 战斗 → 攻击 → 传送 → 施法 → 死亡）
- 传送频率动态递增，多法术连续施放

### 存档系统

- JSON 序列化 + XOR 加密保护存档数据
- FileDataHandler 负责文件读写
- 各系统通过 ISaveManager 接口统一管理存档

## 代码结构

```
Assets/Scripts/
├── Entity.cs                 # 所有角色的基类
├── Player/                   # 玩家状态机（15+ 种状态）
├── Enemy/
│   ├── Enemy.cs              # 敌人基类
│   ├── Skeleton/             # 骷髅 AI
│   ├── Slime/                # 史莱姆 AI
│   ├── Archer/               # 弓箭手 AI
│   ├── Shady/                # 暗影 AI
│   └── DeathBriner/          # Boss AI
├── Skills/                   # 7 种技能 + 技能控制器
├── Items and Inventory/      # 装备背包系统
├── Stats/                    # 属性与伤害计算
├── Save and Load/            # 存档系统（JSON + XOR 加密）
├── UI/                       # 技能树/背包/合成/血条等界面
├── Manager/                  # 全局管理器
├── Effects/                  # 特效系统
└── EffectController/         # 特效控制
```

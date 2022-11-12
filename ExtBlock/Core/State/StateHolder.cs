using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ExtBlock.Core.Registry;

namespace ExtBlock.Core.State
{
    /// <summary>
    /// һ�� StateHolder ������ Owner ��һ��״̬ʵ��, ������ Owner �����������״̬�����Լ��ڵ�ǰ״̬�¸����Ե�ȡֵ, ʾ������ BlockState
    /// </summary>
    /// <typeparam name="O">Owner ����</typeparam>
    /// <typeparam name="S">Holder ����</typeparam>
    public abstract class StateHolder<O, S>
        where O : class, IStateDefiner<O, S>, IRegistryEntry<O>
        where S : StateHolder<O, S>
    {
        /// <summary>
        /// һ�����Ը��� owner �� propertyList ������Ӧ State �Ĺ���ί��
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public delegate S Factory(O owner, ImmutableStatePropertyList? properties);

        public StateHolder(O owner, ImmutableStatePropertyList? propertyList)
        {
            _owner = owner;
            this.propertyList = propertyList;
            _constructed = propertyList == null;
            _hashcodeCache = 31 * owner.StateDefinition.GetHashCode() + (propertyList == null ? 0 : propertyList.PackedIndices % 31);
        }

        /// <summary>
        /// State ��������ȡֵ�б�
        /// </summary>
        public readonly ImmutableStatePropertyList? propertyList;

        /// <summary>
        /// �� State �Ƿ����� Owner ��Ψһ State
        /// </summary>
        public bool IsSingle => propertyList == null;

        /// <summary>
        /// State �� Owner
        /// </summary>
        public O Owner { get => _owner; }
        private readonly O _owner;

        private ImmutableDictionary<StateProperty, ImmutableArray<S>>? _neighbours = null;
        private ImmutableDictionary<StateProperty, S>? _followers = null;

        private readonly int _hashcodeCache;

        /// <summary>
        /// ָʾ State �Ƿ��Ѿ��������
        /// </summary>
        private bool _constructed;

        /// <summary>
        /// Ϊ
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="followers"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetNeighboursAndFollowers(ImmutableDictionary<StateProperty, ImmutableArray<S>> neighbours, ImmutableDictionary<StateProperty, S> followers)
        {
            if(_constructed)
            {
                throw new InvalidOperationException("can't set neighbours and followers for a StateHolder twice");
            }
            Debug.Assert(neighbours != null && followers != null);
            _constructed = true;
            _neighbours = neighbours;
            _followers = followers;
        }

        /// <summary>
        /// ��ȡ�� State ��ָ�� property ��ȡ��ָ���±��ֵ�� neighbour
        /// </summary>
        /// <param name="property"></param>
        /// <param name="valueIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetProperty(StateProperty property, int valueIndex, [NotNullWhen(true)] out S? state)
        {
            if (_neighbours != null &&
                property.IndexIsValid(valueIndex) &&
                _neighbours.TryGetValue(property, out var states))
            {
                state = states[valueIndex];
                return true;
            }
            state = null;
            return false;
        }

        /// <summary>
        /// ��ȡ�� State ��ָ�� property �ϵ� follower
        /// </summary>
        /// <param name="property"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool CycleProperty(StateProperty property, [NotNullWhen(true)] out S? state)
        {
            if(_followers != null && _followers!.TryGetValue(property, out state))
            {
                return true;
            }
            state = null;
            return false;
        }

        public override int GetHashCode()
        {
            return _hashcodeCache;
        }
    }
}

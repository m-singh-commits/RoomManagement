<template>
    <button class="bg-blue-500 hover:bg-blue-700 font-semibold py-1.5 px-2.5 hover:cursor-pointer transition-all shadow-lg rounded-lg duration-150 text-sm text-white" @click="onClick">
        <div class="flex items-center space-x-2 mt-1">
            <div v-if="icon != undefined && icon != ''">
                <Icon :name="icon" class="w-5 h-5" />
            </div>
            <div class="-mt-1">
                {{ label }}
            </div>
        </div>
    </button>
</template>

<script setup lang="ts">
const props = defineProps(['label', 'icon'])
const emit = defineEmits(['click'])

const onClick = () => emit('click')
</script>
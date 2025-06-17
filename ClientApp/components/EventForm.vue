<template>
    <div>
        <div class="text-2xl font-bold mb-4">
            Event
        </div>
        <FormKit
            type="form"
            :actions="false"
            @submit="onSubmit"
        >
            <FormKit
                label="Event Title"
            />

            <Button label="New Event" />
        </FormKit>
    </div>
</template>

<script setup lang="ts">
const onSubmit = () => {

}
</script>
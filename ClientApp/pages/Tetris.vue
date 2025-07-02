<template>
    <div class = "flex flex-col items-center p=4">
        <div class="bg-cyan-300"/>
        <div class="bg-blue-300"/>
        <div class="bg-purple-300"/>
        <div class="bg-orange-300"/>
        <div class="bg-yellow-300"/>
        <div class="bg-green-300"/>
        <div class="bg-red-300"/>
        
        <div class="grid grid-cols-10 gap-0.1 bg-gray-200">
            <div v-for="(row, y) in board" :key="y">
                <div v-for="(cell, x) in row" :key="x" class="w-6 h-6 border border-white rounded" :class="cellClass(cell)">
                    
                </div>
            </div>
        </div>
    </div>

</template>

<script setup lang="ts">
    const ROWS = 20
    const COLS = 10

    const COLORS = ['cyan', 'blue', 'orange', 'yellow', 'green', 'purple', 'red']

    const SHAPES = [
    [[1, 1, 1, 1]],         // I
    [[2, 0, 0], [2, 2, 2]], // J
    [[0, 0, 3], [3, 3, 3]], // L
    [[4, 4], [4, 4]],       // O
    [[0, 5, 5], [5, 5, 0]], // S
    [[0, 6, 0], [6, 6, 6]], // T
    [[7, 7, 0], [0, 7, 7]]  // Z
    ]

    const randomPiece = () => {
        const idx = Math.floor(Math.random() * SHAPES.length)
        return {
            idx,
            shape: SHAPES[idx],
            x: Math.floor(COLS/2) -1,
            y: 0
        }
    }
    const board = reactive(Array.from({length: COLS}, () => Array(ROWS).fill(0)))
    const score = ref()
    const current = reactive(randomPiece())

    const cellClass = (value) => {
        return value != 0 ? `bg-${COLORS[value-1]}-300` : `bg-gray-200`
    }

    const draw = () => {
        current.shape.forEach((row, dy) => {
            row.forEach((val, dx) => {
                if (val) board [current.x +dx][current.y + dy] = current.idx + 1;
            })
        })
    }

    const unDraw = () => {
        current.shape.forEach((row, dy) => {
            row.forEach((val, dx) => {
                if(val) board[current.x + dx][current.y + dy] = 0
            })
        })
    }

    const collide = (xOffset=0, yOffset=0) => {
        return current.shape.some((row, dy) => 
            row.some((val,dx) => {
                if(!val) return false
                const x = current.x + dx + xOffset
                const y = current.y + dy + yOffset

                return (y >= ROWS || x >= COLS || x<0 || y < 0) 
            })
        )
    }

    const rotate= (shape) => {
        return shape[0].map((_,idx) => shape.map(row[idx]).reverse())
    }

    const rotateShape = () => {
        unDraw()
        current.shape = rotate(current.shape);
        draw()
    }

    const drop = () => {
        unDraw()
        if(!collide(0,1))
        {
        current.y += 1
        }
        draw()
    }
    
    const move = (dir) => {
        unDraw()
        if (!collide(dir,0))
            current.x += dir
        draw()
    }

    const handleKey = (event) => {
        if(event.key === 'ArrowUp') rotateShape()
        if(event.key === 'ArrowDown') drop()
        if(event.key === 'ArrowLeft') move(-1)
        if(event.key === 'ArrowRight') move(1)
    }

    const startGame = () => {
        draw()
        setInterval(drop, 500)
        window.addEventListener('keydown', handleKey)
    }

    onMounted(startGame)
    {

    }
</script>